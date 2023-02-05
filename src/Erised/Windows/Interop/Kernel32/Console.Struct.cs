using System.Runtime.InteropServices;

namespace Erised;

internal static partial class Windows
{
    public static partial class Interop
    {
        public static partial class Kernel32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_SCREEN_BUFFER_INFO
            {
                public COORD dwSize;
                public COORD dwCursorPosition;
                public ushort wAttributes;
                public SMALL_RECT srWindow;
                public COORD dwMaximumWindowSize;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct COORD
            {
                public short X;
                public short Y;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct SMALL_RECT
            {
                public short Left;
                public short Top;
                public short Right;
                public short Bottom;
            }

            [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
            public struct INPUT_EVENT_RECORD
            {
                #region KEY_EVENT_RECORD

                [FieldOffset(0)] [MarshalAs(UnmanagedType.Bool)]
                public bool bKeyDown;

                [FieldOffset(4)] public ushort wRepeatCount;

                [FieldOffset(6)] public ushort wVirtualKeyCode;

                [FieldOffset(8)] public ushort wVirtualScanCode;

                [FieldOffset(10)] public ushort UChar; // Union between WCHAR and ASCII char

                [FieldOffset(12)] public uint dwControlKeyState;

                #endregion

                #region MOUSE_EVENT_RECORD

                [FieldOffset(0)] public COORD dwMousePosition;

                [FieldOffset(4)] public uint dwButtonState;

                [FieldOffset(8)] public uint dwControlKeyStateMouse;

                [FieldOffset(12)] public uint dwEventFlags;

                #endregion

                #region FOCUS_EVENT_RECORD

                [FieldOffset(0)] [MarshalAs(UnmanagedType.Bool)]
                public bool bSetFocus;

                #endregion

                #region MENU_EVENT_RECORD

                [FieldOffset(0)] public uint dwCommandId;

                #endregion

                #region WINDOW_BUFFER_SIZE_RECORD

                [FieldOffset(0)] public COORD dwSize;

                #endregion
            }

            public enum EventType : ushort
            {
                FocusEvent = 0x0010,
                KeyEvent = 0x0001,
                MenuEvent = 0x0008,
                MouseEvent = 0x0002,
                WindowBufferSizeEvent = 0x0004
            }

            // Really, this is a union of KeyEventRecords and other types.
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct INPUT_RECORD
            {
                public ushort EventType;
                public INPUT_EVENT_RECORD Event;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_CURSOR_INFO
            {
                public uint dwSize;
                public int bVisible;
            }
        }
    }
}