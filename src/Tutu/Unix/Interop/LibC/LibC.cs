using System.Runtime.InteropServices;

namespace Tutu.Unix.Interop.LibC;

internal static partial class LibC
{
    public delegate void SignalHandler(int signal);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial void cfmakeraw(ref termios termios);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int close(int fd);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int fcntl(int fd, int cmd, int arg);

    public static int ioctl(int fd, syscall_arg request)
        => ioctl(fd, request, 0);
    
    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int ioctl(int fd, syscall_arg request, syscall_arg arg);

    public static int ioctl(int fd, int request, int arg) => ioctl(fd, (syscall_arg)request, arg);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int isatty(nint fd);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int open(Span<byte> pathname, int flags, mode_t mode = default);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int poll(Span<pollfd> fds, ulong_t nfds, int timeout);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static unsafe partial ssize_t read(int fd, void* buf, size_t count);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static unsafe partial ssize_t send(int socket, void* buf, size_t len, int flags);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial void signal(int signum, IntPtr handler);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int socketpair(int domain, int type, int protocol, Span<int> sv);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int tcgetattr(int fd, ref termios p);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial int tcsetattr(int fd, int optional_actions, ref termios p);

    [LibraryImport(LibraryName, SetLastError = true)]
    public static unsafe partial ssize_t write(int fd, void* buffer, ssize_t count);
}
