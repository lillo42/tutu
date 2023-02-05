using static Erised.Windows.Interop.Kernel32;

namespace Erised;

internal static partial class Windows
{
    /// <summary>
    /// This is a wrapper for the locations of a rectangle.
    /// </summary>
    /// <param name="Top"></param>
    /// <param name="Right"></param>
    /// <param name="Left"></param>
    /// <param name="Bottom"></param>
    public readonly record struct WindowPositions(short Top, short Right, short Left, short Bottom)
    {
        internal SMALL_RECT ToSmallRect()
            => new() { Bottom = Bottom, Left = Left, Right = Right, Top = Top };

        public static WindowPositions From(CONSOLE_SCREEN_BUFFER_INFO info)
            => From(info.srWindow);

        private static WindowPositions From(SMALL_RECT rect)
            => new(rect.Top, rect.Right, rect.Left, rect.Bottom);
    }
}