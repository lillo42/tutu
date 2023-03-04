using System.Runtime.InteropServices;

namespace Tutu.Unix.Interop.LibC;

internal static partial class LibC
{
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial void cfmakeraw(ref termios termios);
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int close(int fd);


    [LibraryImport(LibraryName, SetLastError = true)]
    public static unsafe partial int ioctl(int fd, ulong request, void* arg);
    
    public static int ioctl(int fd, int request, int arg) 
    {
        unsafe
        {
            return ioctl(fd, (ulong) request, &arg);
        }
    }
    
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int isatty(nint fd);
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int open(Span<byte> pathname, int flags, mode_t mode = default);
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int poll(Span<pollfd> fds, ulong_t nfds, int timeout);
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static unsafe partial ssize_t read(nint fd, void* buf, size_t count);
    
    [LibraryImport(LibraryName, SetLastError = true)] 
    public static unsafe partial ssize_t send(nint socket, void* buf, size_t len, int flags);
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int socketpair(int domain, int type, int protocol, int[] sv);
    
    [LibraryImport(LibraryName, SetLastError = true)] 
    public static partial int tcgetattr(nint fd, ref termios p);
    
    [LibraryImport(LibraryName, SetLastError = true)] 
    public static partial int tcsetattr(nint fd, int optional_actions, ref termios p);
    
}
