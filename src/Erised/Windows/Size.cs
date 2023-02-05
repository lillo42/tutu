using static Erised.Windows.Interop.Kernel32;

namespace Erised;

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