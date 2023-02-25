using static Tutu.Windows.Interop.Kernel32.Windows.Interop.Kernel32;

namespace Tutu.Windows;

internal static partial class Windows
{
    /// <summary>
    /// This is type represents the size of something in width and height.
    /// </summary>
    public readonly record struct Size(short Width, short Height)
    {
        internal static Size From(COORD coord)
            => new(coord.X, coord.Y);
    }
}