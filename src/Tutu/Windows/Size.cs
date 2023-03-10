using Tutu.Windows.Interop.Kernel32;

namespace Tutu.Windows;

/// <summary>
/// This is type represents the size of something in width and height.
/// </summary>
/// <param name="Width"></param>
/// <param name="Height"></param>
internal readonly record struct Size(short Width, short Height)
{
    public static Size From(COORD coord)
        => new(coord.X, coord.Y);
}
