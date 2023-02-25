using static Tutu.Windows.Interop.Kernel32.Windows.Interop.Kernel32;

namespace Tutu.Windows;

internal static partial class Windows
{
    /// <summary>
    /// This is type represents the position of something on a certain 'x' and 'y'.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    public readonly record struct Coordinate(short X, short Y)
    {
        internal COORD ToCoord() => new() { X = X, Y = Y };

        internal static Coordinate From(COORD coord)
            => new(coord.X, coord.Y);
    }
}