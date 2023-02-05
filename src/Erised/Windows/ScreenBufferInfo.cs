using static Erised.Windows.Interop.Kernel32;

namespace Erised;

internal static partial class Windows
{
    /// <summary>
    /// Information about a console screen buffer.
    ///
    /// This wraps
    /// [`CONSOLE_SCREEN_BUFFER_INFO`](https://docs.microsoft.com/en-us/windows/console/console-screen-buffer-info-str).
    /// </summary>
    /// <param name="Info"></param>
    public readonly record struct ScreenBufferInfo(CONSOLE_SCREEN_BUFFER_INFO Info)
    {
        /// <summary>
        ///  Get the size of the screen buffer.
        ///
        /// Will take `dwSize` from the current screen buffer and convert it into a [`Size`].
        /// </summary>
        public Size BufferSize => Size.From(Info.dwSize);

        /// <summary>
        /// Get the size of the terminal display window.
        ///
        /// Will calculate the width and height from `srWindow` and convert it into a [`Size`].
        /// </summary>
        public Size TerminalSize
            => new((short)(Info.srWindow.Right - Info.srWindow.Left),
                (short)(Info.srWindow.Bottom - Info.srWindow.Top));

        /// <summary>
        ///  Get the position and size of the terminal display window.
        ///
        /// Will take `srWindow` and convert it into the `WindowPositions` type.
        /// </summary>
        public WindowPositions TerminalWindow
            => WindowPositions.From(Info);

        /// <summary>
        /// Get the current attributes of the characters that are being written to the console.
        ///
        /// Will take `wAttributes` from the current screen buffer.
        /// </summary>
        public ushort Attributes
            => Info.wAttributes;

        /// <summary>
        /// Get the current column and row of the terminal cursor in the screen buffer.
        ///
        /// Will take `dwCursorPosition` from the current screen buffer.
        /// </summary>
        public Coordinate CursorPos
            => Coordinate.From(Info.dwCursorPosition);
    }
}