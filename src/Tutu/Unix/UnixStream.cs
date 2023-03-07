using System.Runtime.InteropServices;
using static Tutu.Unix.Interop.LibC.LibC;

namespace Tutu.Unix;

internal class UnixStream
{
    public static (FileDesc, FileDesc) CreateUnixStreamPair()
    {
        var fds = new int[2];
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            if (socketpair(AF_UNIX, SOCK_STREAM, 0, fds) < 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }

            if (ioctl(fds[0], FIOCLEX) < 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
            
            if (ioctl(fds[1], FIOCLEX) < 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
        }
        else
        {
            if (socketpair(AF_UNIX, SOCK_STREAM | SOCK_CLOEXEC, 0, fds) < 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
        }
        
        return (new FileDesc(fds[0], true), new FileDesc(fds[1], true));
    }
}
