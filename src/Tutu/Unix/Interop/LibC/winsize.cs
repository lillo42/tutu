#pragma warning disable CS0649
#pragma warning disable CS8981
namespace Tutu.Unix.Interop.LibC;

internal struct winsize
{
    public ushort ws_row;
    public ushort ws_col;
    public ushort ws_xpixel;
    public ushort ws_ypixel;
}
