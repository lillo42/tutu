using System.Text;
using Tutu.Events;
using static Tutu.Events.Event;
using static Tutu.Events.InternalEvent;

namespace Tutu.Unix;

internal class UnixEventParse
{
    private readonly List<byte> _buffer;
    private readonly Queue<IInternalEvent> _internalEvents;

    public UnixEventParse()
    {
        // This buffer is used for -> 1 <- ANSI escape sequence. Are we
        // aware of any ANSI escape sequence that is bigger? Can we make
        // it smaller?
        //
        // Probably not worth spending more time on this as "there's a plan"
        // to use the anes crate parser.
        _buffer = new List<byte>(256);

        // TTY_BUFFER_SIZE is 1_024 bytes. How many ANSI escape sequences can
        // fit? What is an average sequence length? Let's guess here
        // and say that the average ANSI escape sequence length is 8 bytes. Thus
        // the buffer size should be 1024/8=128 to avoid additional allocations
        // when processing large amounts of data.
        //
        // There's no need to make it bigger, because when you look at the `try_read`
        // method implementation, all events are consumed before the next TTY_BUFFER
        // is processed -> events pushed.
        _internalEvents = new Queue<IInternalEvent>(128);
    }

    public IInternalEvent? Next()
    {
        _internalEvents.TryDequeue(out var ans);
        return ans;
    }

    public void Advance(Span<byte> buffer, bool hasMore)
    {
        for (var idx = 0; idx < buffer.Length; idx++)
        {
            var more = idx + 1 < _buffer.Count || hasMore;
            _buffer.Add(buffer[idx]);

            try
            {
                var @event = ParseEvent(_buffer.ToArray(), more);

                // Event can't be parsed, because we don't have enough bytes for
                // the current sequence. Keep the buffer and process next bytes.
                if (@event != null)
                {
                    _internalEvents.Enqueue(@event);
                    _buffer.Clear();
                }
            }
            catch
            {
                // Event can't be parsed (not enough parameters, parameter is not a number, ...).
                // Clear the buffer and continue with another sequence.
                _buffer.Clear();
            }
        }
    }

// Event parsing
//
// This code are kind of ugly. We have to think about this,
// because it's really not maintainable, no tests, etc.
//
// Every fn returns Result<Option<InputEvent>>
//
// Ok(None) -> wait for more bytes
// Err(_) -> failed to parse event, clear the buffer
// Ok(Some(event)) -> we have event, clear the buffer
    private static IInternalEvent? ParseEvent(Span<byte> buffer, bool isInputAvailable)
    {
        if (buffer.IsEmpty)
        {
            return null;
        }

        if (buffer[0] == '\x1B')
        {
            if (buffer.Length == 1)
            {
                return isInputAvailable ? null : Event(Key(new KeyEvent(KeyCode.Esc, KeyModifiers.None)));
            }

            if (buffer[1] == 'O')
            {
                if (buffer.Length == 2)
                {
                    return null;
                }

                if (buffer[2] == 'A')
                {
                    return Event(Key(new KeyEvent(KeyCode.Up, KeyModifiers.None)));
                }

                if (buffer[2] == 'B')
                {
                    return Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None)));
                }

                if (buffer[2] == 'C')
                {
                    return Event(Key(new KeyEvent(KeyCode.Right, KeyModifiers.None)));
                }

                if (buffer[2] == 'D')
                {
                    return Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.None)));
                }

                if (buffer[2] == 'H')
                {
                    return Event(Key(new KeyEvent(KeyCode.Home, KeyModifiers.None)));
                }

                if (buffer[2] == 'F')
                {
                    return Event(Key(new KeyEvent(KeyCode.End, KeyModifiers.None)));
                }

                if (buffer[2] >= 'P' && buffer[2] <= 'S')
                {
                    const byte p = (byte)'P';
                    return Event(Key(new KeyEvent(KeyCode.F((ushort)(1 + buffer[2] - p)), KeyModifiers.None)));
                }
            }

            if (buffer[1] == '[')
            {
                return ParseCsi(buffer);
            }

            if (buffer[1] == '\x1B')
            {
                var @event = ParseEvent(buffer[1..], isInputAvailable);
                return @event switch
                {
                    null => null,
                    PublicEvent { Event: KeyEventEvent keyEvent } => Event(Key(new KeyEvent(keyEvent.Event.Code,
                        keyEvent.Event.Modifiers | KeyModifiers.Alt))),
                    _ => @event
                };
            }
        }

        if (buffer[0] == '\r')
        {
            return Event(Key(new KeyEvent(KeyCode.Enter, KeyModifiers.None)));
        }

        if (buffer[0] == '\n' && Terminal.Terminal.IsRawModeEnabled)
        {
            return Event(Key(new KeyEvent(KeyCode.Enter, KeyModifiers.None)));
        }

        if (buffer[0] == '\t')
        {
            return Event(Key(new KeyEvent(KeyCode.Tab, KeyModifiers.None)));
        }

        if (buffer[0] == '\x7f')
        {
            return Event(Key(new KeyEvent(KeyCode.Backspace, KeyModifiers.None)));
        }

        if (buffer[0] >= '\x01' && buffer[0] <= '\x1A')
        {
            var key = (char)(buffer[0] - 0x1C + (byte)'4');
            return Event(Key(new KeyEvent(KeyCode.Char(key), KeyModifiers.Control)));
        }

        if (buffer[0] >= '\x1C' && buffer[0] <= '\x1F')
        {
            var key = (char)(buffer[0] - 0x1C + (byte)'4');
            return Event(Key(new KeyEvent(KeyCode.Char(key), KeyModifiers.Control)));
        }

        if (buffer[0] == '\0')
        {
            return Event(Key(new KeyEvent(KeyCode.Char(' '), KeyModifiers.Control)));
        }


        var ch = ParseUt8Char(buffer);
        return Event(Key(new KeyEvent(KeyCode.Char(ch), char.IsUpper(ch) ? KeyModifiers.Shift : KeyModifiers.None)));
    }

    private static char ParseUt8Char(ReadOnlySpan<byte> buffer)
    {
        try
        {
            var s = Encoding.UTF8.GetString(buffer);
            return s[0];
        }
        catch
        {
            // from_utf8 failed, but we have to check if we need more bytes for code point
            // and if all the bytes we have no are valix
            var requiredBytes = buffer[0] switch
            {
                // https://en.wikipedia.org/wiki/UTF-8#Description
                >= 0x00 and <= 0x7F => 1, // 0xxxxxxx
                >= 0xC0 and <= 0xDF => 2, // 110xxxxx 10xxxxxx
                >= 0xE0 and <= 0xEF => 3, // 1110xxxx 10xxxxxx 10xxxxxx
                >= 0xF0 and <= 0xF7 => 4, // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
                _ => throw new IOException("Invalid UTF-8 sequence")
            };

            if (requiredBytes > 1 && buffer.Length > 1)
            {
                foreach (var @byte in buffer[1..])
                {
                    if ((@byte & ~0b0011_1111) != 0b1000_0000)
                    {
                        throw new IOException("Invalid UTF-8 sequence");
                    }
                }
            }

            if (buffer.Length < requiredBytes)
            {
                return default;
            }

            throw new IOException("Invalid UTF-8 sequence");
        }
    }

    private static IInternalEvent? ParseCsiNormalMouse(ReadOnlySpan<byte> buffer)
    {
        // Normal mouse encoding: ESC [ M CB Cx Cy (6 characters only).
        if (buffer.Length < 6)
        {
            return null;
        }

        var cb = (byte)(buffer[3] - 32);
        var (kind, modifiers) = ParseCb(cb);

        // See http://www.xfree86.org/current/ctlseqs.html#Mouse%20Tracking
        // The upper left character position on the terminal is denoted as 1,1.
        // Subtract 1 to keep it synced with cursor
        var cx = (ushort)(unchecked(buffer[4] - 32) - 1);
        var cy = (ushort)(unchecked(buffer[5] - 32) - 1);

        return Event(Mouse(new MouseEvent(kind, cx, cy, modifiers)));
    }

    private static IInternalEvent? ParseCsiRxvtMouse(ReadOnlySpan<byte> buffer)
    {
        // rxvt mouse encoding:
        // ESC [ Cb ; Cx ; Cy ; M

        var s = Encoding.UTF8.GetString(buffer);
        var parts = s.Split(';');

        if (!byte.TryParse(parts[0], out var cb))
        {
            return null;
        }

        cb -= 32;
        var (kind, modifiers) = ParseCb(cb);
        if (!ushort.TryParse(parts[1], out var cx))
        {
            return null;
        }

        if (ushort.TryParse(parts[2], out var cy))
        {
            return null;
        }

        return Event(Mouse(new MouseEvent(kind, --cx, --cy, modifiers)));
    }

    private static IInternalEvent? ParseCsiSpecialKeyCode(ReadOnlySpan<byte> buffer)
    {
        var s = Encoding.UTF8.GetString(buffer[2..^1]);
        var split = s.Split(';');
        if (!byte.TryParse(split[0], out var first))
        {
            return null;
        }

        var modifiers = KeyModifiers.None;
        var kind = KeyEventKind.Press;
        var state = KeyEventState.None;

        var parsed = ModifierAndKindParsed(split.AsSpan()[1..]);
        if (parsed != null)
        {
            modifiers = ParseModifiers(parsed.Value.Item1);
            kind = ParseKeyEventKind(parsed.Value.Item2);
            state = ParseModifiersToState(parsed.Value.Item1);
        }

        var keyCode = first switch
        {
            1 or 7 => KeyCode.Home,
            2 => KeyCode.Insert,
            3 => KeyCode.Delete,
            4 or 8 => KeyCode.End,
            5 => KeyCode.PageUp,
            6 => KeyCode.PageDown,
            >= 11 and <= 15 => KeyCode.F((ushort)(first - 10)),
            >= 17 and <= 21 => KeyCode.F((ushort)(first - 11)),
            >= 23 and <= 26 => KeyCode.F((ushort)(first - 12)),
            >= 28 and <= 29 => KeyCode.F((ushort)(first - 15)),
            >= 31 and <= 34 => KeyCode.F((ushort)(first - 17)),
            _ => null
        };

        if (keyCode is null)
        {
            return null;
        }

        return Event(Key(new KeyEvent(keyCode, modifiers, kind, state)));
    }

    private static KeyEventState ParseModifiersToState(byte mask)
    {
        var modifiersMask = unchecked((byte)(mask - 1));
        var state = KeyEventState.None;

        if ((modifiersMask & 64) != 0)
        {
            state |= KeyEventState.CapsLock;
        }

        if ((modifiersMask & 128) != 0)
        {
            state |= KeyEventState.NumLock;
        }

        return state;
    }

    /// <summary>
    /// Cb is the byte of a mouse input that contains the button being used, the key modifiers being
    /// held and whether the mouse is dragging or not.
    ///
    /// Bit layout of cb, from low to high:
    ///
    /// - button number
    /// - button number
    /// - shift
    /// - meta (alt)
    /// - control
    /// - mouse is dragging
    /// - button number
    /// - button number
    /// </summary>
    /// <param name="cb"></param>
    /// <returns></returns>
    private static (MouseEventKind.IMouseEventKind, KeyModifiers) ParseCb(byte cb)
    {
        var buttonNumber = (cb & 0b0000_0111) | ((cb & 0b1100_0000) >> 4);
        var isDragging = (cb & 0b0010_0000) != 0;

        var kind = (buttonNumber, isDragging) switch
        {
            (0, false) => MouseEventKind.Down(MouseButton.Left),
            (1, false) => MouseEventKind.Down(MouseButton.Middle),
            (2, false) => MouseEventKind.Down(MouseButton.Right),
            (0, true) => MouseEventKind.Drag(MouseButton.Left),
            (1, true) => MouseEventKind.Drag(MouseButton.Middle),
            (2, true) => MouseEventKind.Drag(MouseButton.Right),
            (3, false) => MouseEventKind.Up(MouseButton.Left),
            (3, true) or (4, true) or (5, true) => MouseEventKind.Moved,
            (4, false) => MouseEventKind.ScrollUp,
            (5, false) => MouseEventKind.ScrollDown,
            // We do not support other buttons.
            _ => throw new NotSupportedException("Unsupported mouse button number.")
        };

        var modifiers = KeyModifiers.None;
        if ((cb & 0b0000_0100) == 0b0000_0100)
        {
            modifiers |= KeyModifiers.Shift;
        }

        if ((cb & 0b0000_1000) == 0b0000_1000)
        {
            modifiers |= KeyModifiers.Alt;
        }

        if ((cb & 0b0001_0000) == 0b0001_0000)
        {
            modifiers |= KeyModifiers.Control;
        }

        return (kind, modifiers);
    }

    private static IInternalEvent? ParseCsi(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length == 2)
        {
            return null;
        }

        if (buffer[2] == '[')
        {
            if (buffer.Length == 3)
            {
                return null;
            }

            // NOTE (@imdaveho): cannot find when this occurs;
            // having another '[' after ESC[ not a likely scenario
            if (buffer[3] >= 'A' && buffer[3] <= 'E')
            {
                const byte a = (byte)'A';
                return Event(Key(new KeyEvent(KeyCode.F((ushort)(1 + buffer[3] - a)), KeyModifiers.None)));
            }

            return null;
        }

        if (buffer[2] == 'D')
        {
            return Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.None)));
        }

        if (buffer[2] == 'C')
        {
            return Event(Key(new KeyEvent(KeyCode.Right, KeyModifiers.None)));
        }

        if (buffer[2] == 'A')
        {
            return Event(Key(new KeyEvent(KeyCode.Up, KeyModifiers.None)));
        }

        if (buffer[2] == 'B')
        {
            return Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None)));
        }

        if (buffer[2] == 'H')
        {
            return Event(Key(new KeyEvent(KeyCode.Home, KeyModifiers.None)));
        }

        if (buffer[2] == 'F')
        {
            return Event(Key(new KeyEvent(KeyCode.End, KeyModifiers.None)));
        }

        if (buffer[2] == 'Z')
        {
            return Event(Key(new KeyEvent(KeyCode.Backspace, KeyModifiers.Shift, KeyEventKind.Press)));
        }

        if (buffer[2] == 'M')
        {
            return ParseCsiNormalMouse(buffer);
        }

        if (buffer[2] == '<')
        {
            return ParseCsiSgrMouse(buffer);
        }

        if (buffer[2] == 'I')
        {
            return Event(FocusGained);
        }

        if (buffer[2] == 'O')
        {
            return Event(FocusLost);
        }

        if (buffer[2] == ';')
        {
            return ParseCsiModifierKey(buffer);
        }

        // P, Q, and S for compatibility with Kitty keyboard protocol,
        // as the 1 in 'CSI 1 P' etc. must be omitted if there are no
        // modifiers pressed:
        // https://sw.kovidgoyal.net/kitty/keyboard-protocol/#legacy-functional-keys
        if (buffer[2] == 'P')
        {
            return Event(Key(new KeyEvent(KeyCode.F(1), KeyModifiers.None)));
        }

        if (buffer[2] == 'Q')
        {
            return Event(Key(new KeyEvent(KeyCode.F(2), KeyModifiers.None)));
        }

        if (buffer[2] == 'S')
        {
            return Event(Key(new KeyEvent(KeyCode.F(4), KeyModifiers.None)));
        }

        if (buffer[2] == '?')
        {
            if (buffer[^1] == 'u')
            {
                return ParseCsiKeyboardEnhancementFlags(buffer);
            }

            if (buffer[^1] == 'c')
            {
                return ParseCsiPrimaryDeviceAttributes(buffer);
            }

            return null;
        }

        if (buffer[2] >= '0' && buffer[2] <= '9')
        {
            // Numbered escape code.
            if (buffer.Length == 3)
            {
                return null;
            }

            // The final byte of a CSI sequence can be in the range 64-126, so
            // let's keep reading anything else.
            var lastByte = buffer[^1];
            if (lastByte is >= 64 and <= 126)
            {
                return null;
            }

            if (buffer.StartsWith("\x1B[200~"u8.ToArray().AsSpan()))
            {
                return ParseCSIBracketedPaste(buffer);
            }

            if (lastByte == 'M')
            {
                return ParseCsiRxvtMouse(buffer);
            }

            if (lastByte == '~')
            {
                return ParseCsiSpecialKeyCode(buffer);
            }

            if (lastByte == 'u')
            {
                return ParseCsiUEncodedKeyCode(buffer);
            }

            if (lastByte == 'R')
            {
                return ParseCsiCursorPosition(buffer);
            }

            return ParseCsiModifierKeyCode(buffer);
        }

        return null;
    }

    private static IInternalEvent? ParseCSIBracketedPaste(ReadOnlySpan<byte> buffer)
    {
        if (!buffer.EndsWith("\x1b[201~"u8.ToArray()))
        {
            return null;
        }

        var s = Encoding.UTF8.GetString(buffer[6..^6]);
        return Event(Pasted(s));
    }

    private static IInternalEvent? ParseCsiModifierKeyCode(ReadOnlySpan<byte> buffer)
    {
        var s = Encoding.UTF8.GetString(buffer[2..^1]);
        var split = s.Split(';');

        var modifiers = KeyModifiers.None;
        var kind = KeyEventKind.Press;

        var parsed = ModifierAndKindParsed(split[1..]);
        if (parsed != null)
        {
            modifiers = ParseModifiers(parsed.Value.Item1);
            kind = ParseKeyEventKind(parsed.Value.Item2);
        }
        else if (buffer.Length > 3)
        {
            try
            {
                var ch = Convert.ToChar(buffer[^2]);
                modifiers = ParseModifiers((byte)ch);
            }
            catch
            {
                return null;
            }
        }

        var key = buffer[^1];

        var keyCode = key switch
        {
            (byte)'A' => KeyCode.Up,
            (byte)'B' => KeyCode.Down,
            (byte)'C' => KeyCode.Right,
            (byte)'D' => KeyCode.Left,
            (byte)'F' => KeyCode.End,
            (byte)'H' => KeyCode.Home,
            (byte)'P' => KeyCode.F(1),
            (byte)'Q' => KeyCode.F(2),
            (byte)'R' => KeyCode.F(3),
            (byte)'S' => KeyCode.F(4),
            _ => null
        };

        if (keyCode == null)
        {
            return null;
        }

        return Event(Key(new KeyEvent(keyCode, modifiers, kind)));
    }

    private static IInternalEvent? ParseCsiCursorPosition(ReadOnlySpan<byte> buffer)
    {
        var s = Encoding.UTF8.GetString(buffer[2..^1]);
        var split = s.Split(';');

        if (!ushort.TryParse(split[0], out var y))
        {
            return null;
        }

        if (!ushort.TryParse(split[1], out var x))
        {
            return null;
        }

        return CursorPosition(--x, --y);
    }

    private static IInternalEvent? ParseCsiUEncodedKeyCode(ReadOnlySpan<byte> buffer)
    {
        var s = Encoding.UTF8.GetString(buffer[2..^1]);
        var split = s.Split(';');

        if (!uint.TryParse(split[0], out var codePoint))
        {
            return null;
        }

        var modifiers = KeyModifiers.None;
        var kind = KeyEventKind.Press;
        var state = KeyEventState.None;

        var parsed = ModifierAndKindParsed(split[1..].AsSpan());
        if (parsed != null)
        {
            modifiers = ParseModifiers(parsed.Value.Item1);
            kind = ParseKeyEventKind(parsed.Value.Item2);
            state = ParseModifiersToState(parsed.Value.Item1);
        }

        KeyCode.IKeyCode keyCode;
        KeyEventState stateFromKeyCode = KeyEventState.None;
        var trans = TranslateFunctionalKeyCode(codePoint);
        if (trans != null)
        {
            (keyCode, stateFromKeyCode) = trans.Value;
        }
        else
        {
            char ch;
            try
            {
                ch = Convert.ToChar(codePoint);
            }
            catch
            {
                return null;
            }

            keyCode = ch switch
            {
                '\x1B' => KeyCode.Esc,
                '\r' => KeyCode.Enter,

                // Issue #371: \n = 0xA, which is also the keycode for Ctrl+J. The only reason we get
                // newlines as input is because the terminal converts \r into \n for us. When we
                // enter raw mode, we disable that, so \n no longer has any meaning - it's better to
                // use Ctrl+J. Waiting to handle it here means it gets picked up later
                '\n' when Terminal.Terminal.IsRawModeEnabled => KeyCode.Enter,
                '\t' when modifiers.HasFlag(KeyModifiers.Shift) => KeyCode.BackTab,
                '\t' => KeyCode.Tab,
                '\x7F' => KeyCode.Backspace,
                _ => KeyCode.Char(ch)
            };
        }

        if (keyCode is KeyCode.ModifierKeyCode modifierKeyCode)
        {
            if (modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.LeftAlt)
                || modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.RightAlt))
            {
                modifiers |= KeyModifiers.Alt;
            }

            if (modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.LeftControl)
                || modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.RightControl))
            {
                modifiers |= KeyModifiers.Control;
            }

            if (modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.LeftShift)
                || modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.RightShift))
            {
                modifiers |= KeyModifiers.Shift;
            }

            if (modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.LeftSuper)
                || modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.RightSuper))
            {
                modifiers |= KeyModifiers.Super;
            }

            if (modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.LeftHyper)
                || modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.RightHyper))
            {
                modifiers |= KeyModifiers.Hyper;
            }

            if (modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.LeftMeta)
                || modifierKeyCode.Modifier.HasFlag(ModifierKeyCode.RightMeta))
            {
                modifiers |= KeyModifiers.Meta;
            }
        }

        return Event(Key(new KeyEvent(keyCode, modifiers, kind, state | stateFromKeyCode)));
    }

    private static (KeyCode.IKeyCode, KeyEventState)? TranslateFunctionalKeyCode(uint codepoint)
    {
        var keyCode = codepoint switch
        {
            57399 => KeyCode.Char('0'),
            57400 => KeyCode.Char('1'),
            57401 => KeyCode.Char('2'),
            57402 => KeyCode.Char('3'),
            57403 => KeyCode.Char('4'),
            57404 => KeyCode.Char('5'),
            57405 => KeyCode.Char('6'),
            57406 => KeyCode.Char('7'),
            57407 => KeyCode.Char('8'),
            57408 => KeyCode.Char('9'),
            57409 => KeyCode.Char('.'),
            57410 => KeyCode.Char('/'),
            57411 => KeyCode.Char('*'),
            57412 => KeyCode.Char('-'),
            57413 => KeyCode.Char('+'),
            57414 => KeyCode.Enter,
            57415 => KeyCode.Char('='),
            57416 => KeyCode.Char(','),
            57417 => KeyCode.Left,
            57418 => KeyCode.Right,
            57419 => KeyCode.Up,
            57420 => KeyCode.Down,
            57421 => KeyCode.PageUp,
            57422 => KeyCode.PageDown,
            57423 => KeyCode.Home,
            57424 => KeyCode.End,
            57425 => KeyCode.Insert,
            57426 => KeyCode.Delete,
            57427 => KeyCode.KeypadBegin,
            _ => null
        };

        if (keyCode != null)
        {
            return (keyCode, KeyEventState.Keypad);
        }

        keyCode = codepoint switch
        {
            57358 => KeyCode.CapsLock,
            57359 => KeyCode.ScrollLock,
            57360 => KeyCode.NumLock,
            57361 => KeyCode.PrintScreen,
            57362 => KeyCode.Pause,
            57363 => KeyCode.Menu,
            57376 => KeyCode.F(13),
            57377 => KeyCode.F(14),
            57378 => KeyCode.F(15),
            57379 => KeyCode.F(16),
            57380 => KeyCode.F(17),
            57381 => KeyCode.F(18),
            57382 => KeyCode.F(19),
            57383 => KeyCode.F(20),
            57384 => KeyCode.F(21),
            57385 => KeyCode.F(22),
            57386 => KeyCode.F(23),
            57387 => KeyCode.F(24),
            57388 => KeyCode.F(25),
            57389 => KeyCode.F(26),
            57390 => KeyCode.F(27),
            57391 => KeyCode.F(28),
            57392 => KeyCode.F(29),
            57393 => KeyCode.F(30),
            57394 => KeyCode.F(31),
            57395 => KeyCode.F(32),
            57396 => KeyCode.F(33),
            57397 => KeyCode.F(34),
            57398 => KeyCode.F(35),
            57428 => KeyCode.Media(MediaKeyCode.Play),
            57429 => KeyCode.Media(MediaKeyCode.Pause),
            57430 => KeyCode.Media(MediaKeyCode.PlayPause),
            57431 => KeyCode.Media(MediaKeyCode.Reverse),
            57432 => KeyCode.Media(MediaKeyCode.Stop),
            57433 => KeyCode.Media(MediaKeyCode.FastForward),
            57434 => KeyCode.Media(MediaKeyCode.Rewind),
            57435 => KeyCode.Media(MediaKeyCode.TrackNext),
            57436 => KeyCode.Media(MediaKeyCode.TrackPrevious),
            57437 => KeyCode.Media(MediaKeyCode.Record),
            57438 => KeyCode.Media(MediaKeyCode.LowerVolume),
            57439 => KeyCode.Media(MediaKeyCode.RaiseVolume),
            57440 => KeyCode.Media(MediaKeyCode.MuteVolume),
            57441 => KeyCode.Modifier(ModifierKeyCode.LeftShift),
            57442 => KeyCode.Modifier(ModifierKeyCode.LeftControl),
            57443 => KeyCode.Modifier(ModifierKeyCode.LeftAlt),
            57444 => KeyCode.Modifier(ModifierKeyCode.LeftSuper),
            57445 => KeyCode.Modifier(ModifierKeyCode.LeftHyper),
            57446 => KeyCode.Modifier(ModifierKeyCode.LeftMeta),
            57447 => KeyCode.Modifier(ModifierKeyCode.RightShift),
            57448 => KeyCode.Modifier(ModifierKeyCode.RightControl),
            57449 => KeyCode.Modifier(ModifierKeyCode.RightAlt),
            57450 => KeyCode.Modifier(ModifierKeyCode.RightSuper),
            57451 => KeyCode.Modifier(ModifierKeyCode.RightHyper),
            57452 => KeyCode.Modifier(ModifierKeyCode.RightMeta),
            57453 => KeyCode.Modifier(ModifierKeyCode.IsoLevel3Shift),
            57454 => KeyCode.Modifier(ModifierKeyCode.IsoLevel5Shift),
            _ => null
        };

        if (keyCode != null)
        {
            return (keyCode, KeyEventState.None);
        }

        return null;
    }

    private static IInternalEvent ParseCsiPrimaryDeviceAttributes(ReadOnlySpan<byte> buffer)
    {
        // ESC [ 64 ; attr1 ; attr2 ; ... ; attrn ; c

        // This is a stub for parsing the primary device attributes. This response is not
        // exposed in the crossterm API so we don't need to parse the individual attributes yet.
        // See <https://vt100.net/docs/vt510-rm/DA1.html> 
        return PrimaryDeviceAttributes;
    }

    private static IInternalEvent? ParseCsiKeyboardEnhancementFlags(ReadOnlySpan<byte> buffer)
    {
        // ESC [ ? flags u
        if (buffer.Length < 5)
        {
            return null;
        }

        var bits = buffer[3];
        var flags = Events.KeyboardEnhancementFlags.None;


        if ((bits & 1) != 0)
        {
            flags |= Events.KeyboardEnhancementFlags.DisambiguateEscapeCodes;
        }

        if ((bits & 2) != 0)
        {
            flags |= Events.KeyboardEnhancementFlags.ReportEventTypes;
        }

        // *Note*: this is not yet supported by erised.
        // if ((bits & 4) != 0)
        // {
        //     flags |= KeyboardEnhancementFlags::REPORT_ALTERNATE_KEYS;
        // }

        if ((bits & 8) != 0)
        {
            flags |= Events.KeyboardEnhancementFlags.ReportAllKeysAsEscapeCodes;
        }

        // *Note*: this is not yet supported by crossterm.
        // if bits & 16 != 0 {
        //     flags |= KeyboardEnhancementFlags::REPORT_ASSOCIATED_TEXT;
        // }

        return KeyboardEnhancementFlags(flags);
    }

    private static IInternalEvent? ParseCsiSgrMouse(ReadOnlySpan<byte> buffer)
    {
        if (buffer[^1] != 'm' && buffer[^1] != 'M')
        {
            return null;
        }

        var s = Encoding.UTF8.GetString(buffer[3..^1]);
        var split = s.Split(';').AsSpan();
        var cb = byte.Parse(split[0]);

        var (kind, modifiers) = ParseCb(cb);

        // See http://www.xfree86.org/current/ctlseqs.html#Mouse%20Tracking
        // The upper left character position on the terminal is denoted as 1,1.
        // Subtract 1 to keep it synced with cursor.
        if (!ushort.TryParse(split[1], out var cx))
        {
            return null;
        }

        if (!ushort.TryParse(split[2], out var cy))
        {
            return null;
        }

        cx--;
        cy--;

        // When button 3 in Cb is used to represent mouse release, you can't tell which button was
        // released. SGR mode solves this by having the sequence end with a lowercase m if it's a
        // button release and an uppercase M if it's a button press.
        //
        // We've already checked that the last character is a lowercase or uppercase M at the start of
        // this function, so we just need one if.
        if (buffer[^1] == 'm' && kind is MouseEventKind.MouseDownEventKind down)
        {
            kind = MouseEventKind.Up(down.Button);
        }

        return Event(Mouse(new MouseEvent(kind, cx, cy, modifiers)));
    }


    private static IInternalEvent? ParseCsiModifierKey(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 3)
        {
            throw new IOException("Could not parse event");
        }

        var s = Encoding.UTF8.GetString(buffer[2..^1]);
        var split = s.Split(';').AsSpan();

        KeyModifiers keyModifier;
        var keyEventKind = KeyEventKind.Press;
        var modifiers = ModifierAndKindParsed(split[1..]);
        if (modifiers.HasValue)
        {
            keyModifier = ParseModifiers(modifiers.Value.Item1);
            keyEventKind = ParseKeyEventKind(modifiers.Value.Item2);
        }
        else
        {
            keyModifier = ParseModifiers((char)buffer[^2]);
        }

        var key = buffer[^1];
        var keyCode = key switch
        {
            (byte)'A' => KeyCode.Up,
            (byte)'B' => KeyCode.Down,
            (byte)'C' => KeyCode.Right,
            (byte)'D' => KeyCode.Left,
            (byte)'F' => KeyCode.End,
            (byte)'H' => KeyCode.Home,
            (byte)'P' => KeyCode.F(1),
            (byte)'Q' => KeyCode.F(2),
            (byte)'R' => KeyCode.F(3),
            (byte)'S' => KeyCode.F(4),
            _ => null
        };

        return keyCode == null ? null : Event(Key(new KeyEvent(keyCode, keyModifier, keyEventKind)));
    }

    private static (byte, byte)? ModifierAndKindParsed(ReadOnlySpan<string> iter)
    {
        if (iter.IsEmpty)
        {
            return null;
        }

        var sub = iter[0].Split(':');
        if (sub.Length < 2)
        {
            return null;
        }

        byte.TryParse(sub[0], out var modifiers);
        return byte.TryParse(sub[1], out var kind) ? (modifiers, kind) : ((byte, byte))(modifiers, 1);
    }

    private static KeyEventKind ParseKeyEventKind(ushort kind)
        => kind switch
        {
            1 => KeyEventKind.Press,
            2 => KeyEventKind.Release,
            3 => KeyEventKind.Repeat,
            _ => KeyEventKind.Press
        };

    private static KeyModifiers ParseModifiers(ushort mask)
    {
        var modifierMask = unchecked(mask - 1);
        var modifiers = KeyModifiers.None;
        if ((modifierMask & 1) != 0)
        {
            modifiers |= KeyModifiers.Shift;
        }

        if ((modifierMask & 2) != 0)
        {
            modifiers |= KeyModifiers.Alt;
        }

        if ((modifierMask & 4) != 0)
        {
            modifiers |= KeyModifiers.Control;
        }

        if ((modifierMask & 8) != 0)
        {
            modifiers |= KeyModifiers.Super;
        }

        if ((modifierMask & 16) != 0)
        {
            modifiers |= KeyModifiers.Hyper;
        }

        if ((modifierMask & 32) != 0)
        {
            modifiers |= KeyModifiers.Meta;
        }

        return modifiers;
    }
}