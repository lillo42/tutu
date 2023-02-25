using Tutu.Windows2.Interop.Kernel32;

namespace Tutu.Windows2;

/// <summary>
/// This is a wrapper for the locations of a rectangle. 
/// </summary>
internal struct WindowPositions
{
    public readonly short Top;
    public readonly short Right;
    public readonly short Left;
    public readonly short Bottom;

    public WindowPositions(short top, short right, short left, short bottom)
    {
        Top = top;
        Right = right;
        Left = left;
        Bottom = bottom;
    }

    public SMALL_RECT ToSmallRect()
        => new() { Bottom = Bottom, Left = Left, Right = Right, Top = Top };

    public static WindowPositions From(CONSOLE_SCREEN_BUFFER_INFO info)
        => From(info.srWindow);

    private static WindowPositions From(SMALL_RECT rect)
        => new(rect.Top, rect.Right, rect.Left, rect.Bottom);
}