using Tutu.Windows2.Interop.Kernel32;

namespace Tutu.Windows2;

/// <summary>
/// This is type represents the position of something on a certain 'x' and 'y'.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
internal readonly record struct Coordinate(short X, short Y)
{
    public void Deconstruct(out short x, out short y)
    {
        x = X;
        y = Y;
    }
    
    internal COORD ToCoord() => new() { X = X, Y = Y };

    internal static Coordinate From(COORD coord)
        => new(coord.X, coord.Y);
}