using System.Runtime.InteropServices;
using System.Text;
using Tutu.Events;
using Tutu.Windows.Interop.Kernel32;
using static Tutu.Windows.Interop.User32.User32;

namespace Tutu.Windows;

internal class MouseButtonPressed
{
    public bool Left { get; set; }
    public bool Right { get; set; }
    public bool Middle { get; set; }
}

internal static class WindowsEventParse
{
    public static IEvent? HandleKeyEvent(INPUT_EVENT_RECORD @event, ref ushort? surrogate)
    {
        var key = ParseKeyEventRecord(@event);
        if (key is WindowsKeyEvent @windowsKeyEvent)
        {
            surrogate = null;
            return @windowsKeyEvent.Event;
        }

        if (key is WindowsSurrogate windowsSurrogate)
        {
            var ch = HandleSurrogate(ref surrogate, windowsSurrogate.Surrogate);
            if (ch == null)
            {
                return null;
            }

            var modifiers = From(@event.dwControlKeyState);
            return Event.Key(new KeyEvent(KeyCode.Char(ch.Value), modifiers));
        }

        return null;
    }

    public static IEvent? HandleMouseEvent(INPUT_EVENT_RECORD mouseEvent, MouseButtonPressed buttonPressed)
    {
        var @event = ParseMouseEventRecord(mouseEvent, buttonPressed);

        buttonPressed.Left = IsLeft(mouseEvent.dwButtonState);
        buttonPressed.Right = IsRight(mouseEvent.dwButtonState);
        buttonPressed.Middle = IsMiddle(mouseEvent.dwButtonState);

        return @event;
    }

    private static IEvent? ParseMouseEventRecord(INPUT_EVENT_RECORD @event, MouseButtonPressed pressed)
    {
        var modifiers = From(@event.dwControlKeyStateMouse);
        var xpos = @event.dwMousePosition.X;
        var ypos = ParseRelativeY(@event.dwMousePosition.Y);

        var buttonState = @event.dwButtonState;
        var eventFlags = (EventFlags)@event.dwEventFlags;

        MouseEventKind.IMouseEventKind? kind;
        if (eventFlags == EventFlags.PressOrRelease)
        {
            if (IsLeft(buttonState) && !pressed.Left)
            {
                kind = MouseEventKind.Down(MouseButton.Left);
            }
            else if (IsLeft(buttonState) && pressed.Left)
            {
                kind = MouseEventKind.Up(MouseButton.Left);
            }
            else if (IsRight(buttonState) && !pressed.Right)
            {
                kind = MouseEventKind.Down(MouseButton.Right);
            }
            else if (IsRight(buttonState) && pressed.Right)
            {
                kind = MouseEventKind.Up(MouseButton.Right);
            }
            else if (IsMiddle(buttonState) && !pressed.Middle)
            {
                kind = MouseEventKind.Down(MouseButton.Middle);
            }
            else if (IsMiddle(buttonState) && pressed.Middle)
            {
                kind = MouseEventKind.Up(MouseButton.Middle);
            }
            else
            {
                return null;
            }
        }
        else if (eventFlags == EventFlags.MouseMoved)
        {
            var button = MouseButton.Left;
            if (IsRight(buttonState))
            {
                button = MouseButton.Right;
            }
            else if (IsMiddle(buttonState))
            {
                button = MouseButton.Middle;
            }

            kind = IsReleased(buttonState) ? MouseEventKind.Moved : MouseEventKind.Drag(button);
        }
        else if (eventFlags == EventFlags.MouseWheeled)
        {
            // Vertical scroll
            // from https://docs.microsoft.com/en-us/windows/console/mouse-event-record-str
            // if `button_state` is negative then the wheel was rotated backward, toward the user.
            if (IsScrollDown(buttonState))
            {
                kind = MouseEventKind.ScrollDown;
            }
            else if (IsScrollDown(buttonState))
            {
                kind = MouseEventKind.ScrollUp;
            }
            else
            {
                kind = null;
            }
        }
        else
        {
            kind = null;
        }

        return kind == null ? null : Event.Mouse(new MouseEvent(kind, xpos, ypos, modifiers));

        static short ParseRelativeY(short y)
        {
            var size = ScreenBuffer.CurrentOutput.Info.TerminalWindow;
            return (short)(y - size.Top);
        }
    }

    private static bool IsLeft(int controlState) => HasState((uint)controlState, FromLeft1StButtonPressed);

    private static bool IsReleased(int controlState) => controlState == 0;

    private static bool IsRight(int controlState) => HasState((uint)controlState,
        RightmostButtonPressed | FromLeft3RdButtonPressed | FromLeft4ThButtonPressed);

    private static bool IsMiddle(int controlState) => HasState((uint)controlState, FromLeft2NdButtonPressed);

    private static bool IsScrollUp(int controlState) => controlState > 0;

    private static bool IsScrollDown(int controlState) => controlState < 0;

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

    private static IWindowsKeyEvent? ParseKeyEventRecord(INPUT_EVENT_RECORD keyEvent)
    {
        var modifiers = From(keyEvent.dwControlKeyState);
        var virtualKeyCode = (int)keyEvent.wVirtualKeyCode;
        var kind = keyEvent.bKeyDown ? KeyEventKind.Press : KeyEventKind.Release;

        // We normally ignore all key release events, but we will make an exception for an Alt key
        // release if it carries a u_char value, as this indicates an Alt code.
        var isAltCode = virtualKeyCode == VK_MENU && !keyEvent.bKeyDown && keyEvent.UChar != 0;
        if (isAltCode)
        {
            var utf16 = keyEvent.UChar;
            if (utf16 is >= 0xD800 and <= 0xDBFF)
            {
                return new WindowsSurrogate(utf16);
            }

            // Unwrap is safe: We tested for surrogate values above and those are the only
            // u16 values that are invalid when directly interpreted as unicode scalar
            // values.
            var ch = Convert.ToChar(utf16);
            var keyCode = KeyCode.Char(ch);
            var @event = Event.Key(new KeyEvent(keyCode, modifiers, kind));
            return new WindowsKeyEvent(@event);
        }

        // Don't generate events for numpad key presses when they're producing Alt codes.
        var isNumpadNumericKey = virtualKeyCode is >= VK_NUMPAD0 and <= VK_NUMPAD9;
        var isOnlyAltModifier = modifiers.HasFlag(KeyModifiers.Alt) &&
                                (!modifiers.HasFlag(KeyModifiers.Shift) || !modifiers.HasFlag(KeyModifiers.Control));

        if (isNumpadNumericKey && isOnlyAltModifier)
        {
            return null;
        }

        KeyCode.IKeyCode? parse;
        if (virtualKeyCode is VK_SHIFT or VK_CONTROL or VK_MENU)
        {
            parse = null;
        }
        else if (virtualKeyCode == VK_BACK)
        {
            parse = KeyCode.Backspace;
        }
        else if (virtualKeyCode == VK_ESCAPE)
        {
            parse = KeyCode.Esc;
        }
        else if (virtualKeyCode == VK_RETURN)
        {
            parse = KeyCode.Enter;
        }
        else if (virtualKeyCode is >= VK_F1 and <= VK_F24)
        {
            parse = KeyCode.F((byte)(virtualKeyCode - 111));
        }
        else if (virtualKeyCode == VK_LEFT)
        {
            parse = KeyCode.Left;
        }
        else if (virtualKeyCode == VK_UP)
        {
            parse = KeyCode.Up;
        }
        else if (virtualKeyCode == VK_RIGHT)
        {
            parse = KeyCode.Right;
        }
        else if (virtualKeyCode == VK_DOWN)
        {
            parse = KeyCode.Down;
        }
        else if (virtualKeyCode == VK_PRIOR)
        {
            parse = KeyCode.PageUp;
        }
        else if (virtualKeyCode == VK_NEXT)
        {
            parse = KeyCode.PageDown;
        }
        else if (virtualKeyCode == VK_END)
        {
            parse = KeyCode.End;
        }
        else if (virtualKeyCode == VK_HOME)
        {
            parse = KeyCode.Home;
        }
        else if (virtualKeyCode == VK_DELETE)
        {
            parse = KeyCode.Delete;
        }
        else if (virtualKeyCode == VK_INSERT)
        {
            parse = KeyCode.Insert;
        }
        else if (virtualKeyCode == VK_TAB && modifiers.HasFlag(KeyModifiers.Shift))
        {
            parse = KeyCode.BackTab;
        }
        else if (virtualKeyCode == VK_TAB)
        {
            parse = KeyCode.Tab;
        }
        else
        {
            var utf16 = keyEvent.UChar;
            if (utf16 is >= 0x00 and <= 0x1f)
            {
                parse = null;

                // Some key combinations generate either no u_char value or generate control
                // codes. To deliver back a KeyCode::Char(...) event we want to know which
                // character the key normally maps to on the user's keyboard layout.
                // The keys that intentionally generate control codes (ESC, ENTER, TAB, etc.)
                // are handled by their virtual key codes above.
                var c = GetCharForKey(keyEvent);
                if (c != null)
                {
                    parse = KeyCode.Char(c.Value);
                }
            }
            else if (utf16 is >= 0xD800 and <= 0xDFFF)
            {
                return new WindowsSurrogate(utf16);
            }
            else
            {
                // Unwrap is safe: We tested for surrogate values above and those are the only
                // u16 values that are invalid when directly interpreted as unicode scalar
                // values.
                var ch = Convert.ToChar(utf16);
                parse = KeyCode.Char(ch);
            }
        }

        if (parse != null)
        {
            return new WindowsKeyEvent(Event.Key(new KeyEvent(parse, modifiers, kind)));
        }

        return null;
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
    private static char? GetCharForKey(INPUT_EVENT_RECORD keyEvent)
    {
        const int dontChangeKernelKeyboardState = 0x4;
        var virtualKeyCode = keyEvent.wVirtualKeyCode;
        var virtualScanCode = keyEvent.wVirtualScanCode;
        var keyState = new byte[256];

        var utf16 = "";

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
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        var foregroundWindowThread = GetWindowThreadProcessId(foregroundWindow, out _);
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        var activeKeyboardLayout = GetKeyboardLayout(foregroundWindowThread);
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());

        var ret = ToUnicodeEx(virtualKeyCode, virtualScanCode, keyState, utf16, 2, dontChangeKernelKeyboardState,
            activeKeyboardLayout);

        // -1 indicates a dead key.
        // 0 indicates no character for this key.
        if (ret < 1 || utf16.Length == 0)
        {
            return null;
        }

        var isShiftPressed = HasState(keyEvent.dwControlKeyState, ShiftPressed);
        var isCapslockOn = HasState(keyEvent.dwControlKeyState, CapslockOn);
        var isUpperCase = isShiftPressed ^ isCapslockOn;
        if (isUpperCase)
        {
            utf16 = utf16.ToUpper();
        }

        return utf16[0];
    }

    private static bool HasState(uint currentState, uint state) => (currentState & state) != 0;

    private const int FromLeft1StButtonPressed = 0x0001;
    private const int FromLeft2NdButtonPressed = 0x0004;
    private const int FromLeft3RdButtonPressed = 0x0008;
    private const int FromLeft4ThButtonPressed = 0x0010;
    private const int RightmostButtonPressed = 0x0002c;

    private const uint ShiftPressed = 0x0010;
    private const uint CapslockOn = 0x0080;

    private const uint RightAltPressed = 0x0001;
    private const uint LeftAltPressed = 0x0002;

    private const uint RightCtrlPressed = 0x0004;
    private const uint LeftCtrlPressed = 0x0008;

    private const int VK_BACK = 0x08;
    private const int VK_TAB = 0x09;
    private const int VK_SHIFT = 0x10;
    private const int VK_CONTROL = 0x11;
    private const int VK_MENU = 0x12;
    private const int VK_PRIOR = 0x21;
    private const int VK_NEXT = 0x22;
    private const int VK_END = 0x23;
    private const int VK_HOME = 0x24;
    private const int VK_LEFT = 0x25;
    private const int VK_UP = 0x26;
    private const int VK_RIGHT = 0x27;
    private const int VK_DOWN = 0x28;
    private const int VK_NUMPAD0 = 0x60;
    private const int VK_NUMPAD1 = 0x61;
    private const int VK_NUMPAD2 = 0x62;
    private const int VK_NUMPAD3 = 0x63;
    private const int VK_NUMPAD4 = 0x64;
    private const int VK_NUMPAD5 = 0x65;
    private const int VK_NUMPAD6 = 0x66;
    private const int VK_NUMPAD7 = 0x67;
    private const int VK_NUMPAD8 = 0x68;
    private const int VK_NUMPAD9 = 0x69;
    private const int VK_F1 = 0x70;
    private const int VK_F2 = 0x71;
    private const int VK_F3 = 0x72;
    private const int VK_F4 = 0x73;
    private const int VK_F5 = 0x74;
    private const int VK_F6 = 0x75;
    private const int VK_F7 = 0x76;
    private const int VK_F8 = 0x77;
    private const int VK_F9 = 0x78;
    private const int VK_F10 = 0x79;
    private const int VK_F11 = 0x7A;
    private const int VK_F12 = 0x7B;
    private const int VK_F13 = 0x7C;
    private const int VK_F14 = 0x7D;
    private const int VK_F15 = 0x7E;
    private const int VK_F16 = 0x7F;
    private const int VK_F17 = 0x80;
    private const int VK_F18 = 0x81;
    private const int VK_F19 = 0x82;
    private const int VK_F20 = 0x83;
    private const int VK_F21 = 0x84;
    private const int VK_F22 = 0x85;
    private const int VK_F23 = 0x86;
    private const int VK_F24 = 0x87;
    private const int VK_ESCAPE = 0x1B;
    private const int VK_RETURN = 0x0D;
    private const int VK_INSERT = 0x2D;
    private const int VK_DELETE = 0x2E;

    public interface IWindowsKeyEvent
    {
    }

    public record WindowsKeyEvent(IEvent Event) : IWindowsKeyEvent;

    public record WindowsSurrogate(ushort Surrogate) : IWindowsKeyEvent;
}

/// <summary>
/// The type of mouse event.
/// If this value is zero, it indicates a mouse button being pressed or released.
/// Otherwise, this member is one of the following values.
///
/// <see href="https://docs.microsoft.com/en-us/windows/console/mouse-event-record-str#members">Ms Docs</see>
/// </summary>
public enum EventFlags
{
    PressOrRelease = 0x0000,

    /// <summary>
    /// The second click (button press) of a double-click occurred. The first click is returned as a regular button-press event.
    /// </summary>
    DoubleClick = 0x0002,

    /// <summary>
    /// The horizontal mouse wheel was moved.
    /// </summary>
    MouseHwheeled = 0x0008,

    /// <summary>
    /// If the high word of the dwButtonState member contains a positive value, the wheel was rotated to the right. Otherwise, the wheel was rotated to the left.
    /// </summary>
    MouseMoved = 0x0001,

    /// <summary>
    /// A change in mouse position occurred.
    /// The vertical mouse wheel was moved, if the high word of the dwButtonState member contains a positive value, the wheel was rotated forward, away from the user.
    /// Otherwise, the wheel was rotated backward, toward the user.
    /// </summary>
    MouseWheeled = 0x0004,

    /// <summary>
    /// This button state is not recognized.
    /// </summary>
    Unknown = 0x0021,
}