using Tutu.Windows2.Interop.Kernel32;

namespace Tutu.Windows2;

/// <summary>
/// This is type represents the size of something in width and height.
/// </summary>
internal readonly struct Size
{
    public readonly short Width;
    public readonly short Height;

    public Size(short width, short height)
    {
        Width = width;
        Height = height;
    }

    public static Size From(COORD coord)
        => new(coord.X, coord.Y);
}