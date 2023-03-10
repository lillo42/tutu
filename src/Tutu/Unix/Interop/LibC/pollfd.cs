#pragma warning disable CS0649
#pragma warning disable CS8981
namespace Tutu.Unix.Interop.LibC;

internal struct pollfd
{
    public int fd;
    public short events;
    public short revents;
}
