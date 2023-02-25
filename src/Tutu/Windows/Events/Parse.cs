/*
using System.Runtime.InteropServices;
using System.Text;
using Tutu.Events;
using static Tutu.Windows.Interop.Kernel32.Windows.Interop.Kernel32;
using static Tutu.Windows.Interop.User32.Windows.Interop.User32;

namespace Tutu.Windows.Events;

internal static partial class Windows
{
    public partial class WindowsEventStream
    {
        private static bool HasState(uint currentState, uint state) => (currentState & state) != 0;

        private static KeyModifiers From(uint controlState)
        {
            var shift = HasState(controlState, ShiftPressed);
            var alt = HasState(controlState, LeftAltPressed | RightAltPressed);
            var control = HasState(controlState, LeftCtrlPressed | RightCtrlPressed);

            var modifier = KeyModifiers.None;
            if (shift)
            {
                modifier |= KeyModifiers.Shift;
            }

            if (control)
            {
                modifier |= KeyModifiers.Control;
            }

            if (alt)
            {
                modifier |= KeyModifiers.Alt;
            }

            return modifier;
        }

        public static Tutu.Events.Event.KeyEvent? HandleKeyEvent(INPUT_EVENT_RECORD @event, ref ushort? surrogateBuffer)
        {
            var parse = ParseKeyEventRecord(@event);
            if (parse.keyEvent != null)
            {
                return Tutu.Events.Event.Key(parse.keyEvent.Value);
            }

            if (parse.surrogate != null)
            {
                var ch = HandleSurrogate(ref surrogateBuffer, parse.surrogate.Value);
                if (ch == null)
                {
                    return null;
                }
                
                var modifiers = From(@event.dwControlKeyState);
                return Tutu.Events.Event.Key(new KeyEvent(KeyCode.Char(ch.Value), modifiers));
            }

            return null;
        }

        private const int FromLeft1StButtonPressed = 0x0001;
        private const int FromLeft2NdButtonPressed = 0x0004;
        private const int FromLeft3RdButtonPressed = 0x0008;
        private const int FromLeft4ThButtonPressed = 0x0010;
        private const int RightmostButtonPressed = 0x0002;

        public static IEvent? HandleMouseEvent(INPUT_EVENT_RECORD @event, ref MouseButtonPressed pressed)
        {
            var modifiers = From(@event.dwControlKeyStateMouse);

            var xpos = (ushort)@event.dwMousePosition.X;
            var ypos = (ushort)Cursor.Windows.ParseRelativeY(@event.dwMousePosition.Y);

            var kind = Convert(@event, ref pressed);
            if (kind != null)
            {
                return Tutu.Events.Event.Mouse(new MouseEvent(kind, xpos, ypos, modifiers));
            }

            return null;

            static MouseEventKind.IMouseEventKind? Convert(INPUT_EVENT_RECORD @event, ref MouseButtonPressed pressed)
            {
                var eventFlags = (Tutu.Windows.Windows.EventFlags)@event.dwEventFlags;

                var isReleased = @event.dwButtonState == 0;
                var isScrollDown = @event.dwButtonState < 1;
                var isScrollUp = @event.dwButtonState > 1;
                var isLeft = IsButton(@event.dwButtonState, FromLeft1StButtonPressed);
                var isRight = IsButton(@event.dwButtonState,
                    RightmostButtonPressed | FromLeft3RdButtonPressed | FromLeft4ThButtonPressed);
                var isMiddle = IsButton(@event.dwButtonState, FromLeft2NdButtonPressed);

                pressed = new MouseButtonPressed(isLeft, isRight, isMiddle);

                if (eventFlags == Tutu.Windows.Windows.EventFlags.PressOrRelease)
                {
                    if (isLeft && !pressed.Left)
                    {
                        return MouseEventKind.Down(MouseButton.Left);
                    }
                    else if (!isLeft && pressed.Left)
                    {
                        return MouseEventKind.Up(MouseButton.Left);
                    }
                    else if (isRight && !pressed.Right)
                    {
                        return MouseEventKind.Down(MouseButton.Right);
                    }
                    else if (!isRight && pressed.Right)
                    {
                        return MouseEventKind.Up(MouseButton.Right);
                    }
                    else if (isMiddle && !pressed.Middle)
                    {
                        return MouseEventKind.Down(MouseButton.Middle);
                    }
                    else if (!isMiddle && pressed.Middle)
                    {
                        return MouseEventKind.Up(MouseButton.Middle);
                    }
                    else
                    {
                        return null;
                    }
                }

                if (eventFlags == Tutu.Windows.Windows.EventFlags.MouseMoved)
                {
                    if (isReleased)
                    {
                        return MouseEventKind.Moved;
                    }

                    var button = MouseButton.Left;
                    if (isRight)
                    {
                        button = MouseButton.Right;
                    }
                    else if (isMiddle)
                    {
                        button = MouseButton.Middle;
                    }

                    return MouseEventKind.Drag(button);
                }

                if (eventFlags == Tutu.Windows.Windows.EventFlags.MouseWheeled)
                {
                    // Vertical scroll
                    // from https://docs.microsoft.com/en-us/windows/console/mouse-event-record-str
                    // if `button_state` is negative then the wheel was rotated backward, toward the user.   
                    if (isScrollDown)
                    {
                        return MouseEventKind.ScrollDown;
                    }

                    if (isScrollUp)
                    {
                        return MouseEventKind.ScrollUp;
                    }
                }

                return null;
            }

            static bool IsButton(uint state, uint button) => (state & button) != 0;
        }

        public readonly record struct MouseButtonPressed(bool Left, bool Right, bool Middle);

        private static char? HandleSurrogate(ref ushort? surrogate, ushort newSurrogate)
        {
            if (surrogate == null)
            {
                surrogate = newSurrogate;
                return null;
            }

            var bytes = Encoding.Unicode.GetBytes(new string(new[] { (char)surrogate.Value, (char)newSurrogate }));
            surrogate = null;
            return Encoding.Unicode.GetString(bytes)[0];
        }

        private const uint VK_MENU = 0x12;
        private const uint VK_SHIFT = 0x10;
        private const uint VK_CONTROL = 0x11;
        private const uint VK_BACK = 0x08;
        private const uint VK_ESCAPE = 0x1B;
        private const uint VK_RETURN = 0x0D;
        private const uint VK_F1 = 0x70;
        private const uint VK_F24 = 0x87;
        private const uint VK_LEFT = 0x25;
        private const uint VK_UP = 0x26;
        private const uint VK_RIGHT = 0x27;
        private const uint VK_DOWN = 0x28;
        private const uint VK_PRIOR = 0x21;
        private const uint VK_NEXT = 0x22;
        private const uint VK_HOME = 0x24;
        private const uint VK_END = 0x23;
        private const uint VK_DELETE = 0x2E;
        private const uint VK_INSERT = 0x2D;
        private const uint VK_TAB = 0x09;

        private const uint ShiftPressed = 0x0010;
        private const uint CapslockOn = 0x0080;

        private const uint RightAltPressed = 0x0001;
        private const uint LeftAltPressed = 0x0002;

        private const uint RightCtrlPressed = 0x0004;
        private const uint LeftCtrlPressed = 0x0008;

        private static (KeyEvent? keyEvent, ushort? surrogate) ParseKeyEventRecord(INPUT_EVENT_RECORD @event)
        {
            var modifiers = From(@event.dwControlKeyState);
            var virtualKeyCode = (int)@event.wVirtualKeyCode;

            // We normally ignore all key release events, but we will make an exception for an Alt key
            // release if it carries a u_char value, as this indicates an Alt code.
            var isAltCode = virtualKeyCode == VK_MENU && !@event.bKeyDown && @event.UChar != 0;

            if (isAltCode)
            {
                var utf16 = @event.UChar;
                if (utf16 is >= 0xD800 and <= 0xDFFF)
                {
                    return (null, utf16);
                }

                var ch = (char)utf16;
                var keyCode = KeyCode.Char(ch);
                var keyEvent = new KeyEvent(keyCode, modifiers);
                return (keyEvent, null);
            }

            // Don't generate events for numpad key presses when they're producing Alt codes.
            var isNumpadNumericKey = virtualKeyCode is >= 0x60 and <= 0x69;
            var isOnlyAltModifier = modifiers.HasFlag(KeyModifiers.Alt) &&
                                    !modifiers.HasFlag(KeyModifiers.Shift | KeyModifiers.Control);
            if ((isOnlyAltModifier && isNumpadNumericKey) || !@event.bKeyDown)
            {
                return (null, null);
            }

            KeyCode.IKeyCode? parseResult = null;
            if (virtualKeyCode == VK_SHIFT || virtualKeyCode == VK_CONTROL || virtualKeyCode == VK_MENU)
            {
                parseResult = null;
            }
            else if (virtualKeyCode == VK_BACK)
            {
                parseResult = KeyCode.Backspace;
            }
            else if (virtualKeyCode == VK_ESCAPE)
            {
                parseResult = KeyCode.Esc;
            }
            else if (virtualKeyCode == VK_RETURN)
            {
                parseResult = KeyCode.Enter;
            }
            else if (virtualKeyCode >= VK_F1 && virtualKeyCode <= VK_F24)
            {
                parseResult = KeyCode.F((ushort)(virtualKeyCode - 111));
            }
            else if (virtualKeyCode == VK_LEFT)
            {
                parseResult = KeyCode.Left;
            }
            else if (virtualKeyCode == VK_UP)
            {
                parseResult = KeyCode.Up;
            }
            else if (virtualKeyCode == VK_RIGHT)
            {
                parseResult = KeyCode.Right;
            }
            else if (virtualKeyCode == VK_DOWN)
            {
                parseResult = KeyCode.Down;
            }
            else if (virtualKeyCode == VK_PRIOR)
            {
                parseResult = KeyCode.PageUp;
            }
            else if (virtualKeyCode == VK_NEXT)
            {
                parseResult = KeyCode.PageDown;
            }
            else if (virtualKeyCode == VK_HOME)
            {
                parseResult = KeyCode.Home;
            }
            else if (virtualKeyCode == VK_END)
            {
                parseResult = KeyCode.End;
            }
            else if (virtualKeyCode == VK_DELETE)
            {
                parseResult = KeyCode.Delete;
            }
            else if (virtualKeyCode == VK_INSERT)
            {
                parseResult = KeyCode.Insert;
            }
            else if (virtualKeyCode == VK_TAB)
            {
                parseResult = modifiers.HasFlag(KeyModifiers.Shift) ? KeyCode.BackTab : KeyCode.Tab;
            }
            else
            {
                var utf16 = @event.UChar;
                if (utf16 is 0x00 and <= 0x1F)
                {
                    var ch = GetCharForKey(@event);
                    if (ch != null)
                    {
                        parseResult = KeyCode.Char(ch.Value);
                    }
                }
                else if (utf16 is >= 0xD800 and <= 0xDFFF)
                {
                    return (null, utf16);
                }
                else
                {
                    var ch = (char)utf16;
                    parseResult = KeyCode.Char(ch);
                }
            }

            if (parseResult != null)
            {
                var keyEvent = new KeyEvent(parseResult, modifiers);
                return (keyEvent, null);
            }

            return (null, null);
        }


        // Attempts to return the character for a key event accounting for the user's keyboard layout.
        // The returned character (if any) is capitalized (if applicable) based on shift and capslock state.
        // Returns None if the key doesn't map to a character or if it is a dead key.
        // We use the *currently* active keyboard layout (if it can be determined). This layout may not
        // correspond to the keyboard layout that was active when the user typed their input, since console
        // applications get their input asynchronously from the terminal. By the time a console application
        // can process a key input, the user may have changed the active layout. In this case, the character
        // returned might not correspond to what the user expects, but there is no way for a console
        // application to know what the keyboard layout actually was for a key event, so this is our best
        // effort. If a console application processes input in a timely fashion, then it is unlikely that a
        // user has time to change their keyboard layout before a key event is processed.
        private static char? GetCharForKey(INPUT_EVENT_RECORD @event)
        {
            const uint dontChangeKernelKeyboardState = 0x4;
            var virtualKeyCode = (uint)@event.wVirtualKeyCode;
            var virtualScanCode = (uint)@event.wVirtualScanCode;
            var keyState = new byte[256];

            // Best-effort attempt at determining the currently active keyboard layout.
            // At the time of writing, this works for a console application running in Windows Terminal, but
            // doesn't work under a Conhost terminal. For Conhost, the window handle returned by
            // GetForegroundWindow() does not appear to actually be the foreground window which has the
            // keyboard layout associated with it (or perhaps it is, but also has special protection that
            // doesn't allow us to query it).
            // When this determination fails, the returned keyboard layout handle will be null, which is an
            // acceptable input for ToUnicodeEx, as that argument is optional. In this case ToUnicodeEx
            // appears to use the keyboard layout associated with the current thread, which will be the
            // layout that was inherited when the console application started (or possibly when the current
            // thread was spawned). This is then unfortunately not updated when the user changes their
            // keyboard layout in the terminal, but it's what we get.
            var foregroundWindow = GetForegroundWindow();

            var error = Marshal.GetLastPInvokeError();
            if (error != 0)
            {
                throw Tutu.Windows.Windows.GetExceptionForWin32Error(error);
            }

            var foregroundThread = GetWindowThreadProcessId(foregroundWindow, out _);
            error = Marshal.GetLastPInvokeError();
            if (error != 0)
            {
                throw Tutu.Windows.Windows.GetExceptionForWin32Error(error);
            }

            var activeKeyboardLayout = GetKeyboardLayout(foregroundThread);
            error = Marshal.GetLastPInvokeError();
            if (error != 0)
            {
                throw Tutu.Windows.Windows.GetExceptionForWin32Error(error);
            }

            var utf16 = " ";
            var unix = ToUnicodeEx(virtualKeyCode, virtualScanCode, keyState, utf16, 2,
                dontChangeKernelKeyboardState, activeKeyboardLayout);

            // -1 indicates a dead key.
            // 0 indicates no character for this key.
            if (unix < 1 || utf16.Length == 0)
            {
                return null;
            }

            var isShiftPressed = HasState(@event.dwControlKeyState, ShiftPressed);
            var isCapslockOn = HasState(@event.dwControlKeyState, CapslockOn);
            var isUpperCase = isShiftPressed ^ isCapslockOn;
            if (isUpperCase)
            {
                return char.ToUpperInvariant(utf16[0]);
            }

            return utf16[0];
        }
    }
}
*/