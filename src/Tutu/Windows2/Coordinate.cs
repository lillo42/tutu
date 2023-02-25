using Tutu.Windows2.Interop.Kernel32;

namespace Tutu.Windows2;

/// <summary>
/// This is type represents the position of something on a certain 'x' and 'y'.
/// </summary>
internal readonly struct Coordinate
{
    public readonly short X;
    public readonly short Y;

    public Coordinate(short x, short y)
    {
        X = x;
        Y = y;
    }

    public void Deconstruct(out short x, out short y)
    {
        x = X;
        y = Y;
    }
    
    internal COORD ToCoord() => new() { X = X, Y = Y };

    internal static Coordinate From(COORD coord)
        => new(coord.X, coord.Y);
}