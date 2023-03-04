﻿namespace Tutu.Unix.Interop.LibC;

internal static partial class LibC
{
    private const string LibraryName = "libc";
    // private const string LibraryName = "libSystem.dylib";
    
    public const int AF_UNIX = 1;
    public const int EAGAIN = 11;
    public const int EINTR = 4;
    public const int EWOULDBLOCK = 11;
    public const int FIONBIO = 21537;
    public const int O_RDWR = 2;
    public const short POLLIN = 0x1;
    public const int SIGWINCH = 28;
    public const int SOCK_CLOEXEC = 524288;
    public const int SOCK_STREAM = 1;
    public const int STDIN_FILENO = 0;
    public const int TCSANOW = 0;
    public const int TIOCGWINSZ = 21523;
}
