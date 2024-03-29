﻿using System.Runtime.InteropServices;
using Tutu.Unix.Extensions;
using Tutu.Unix.Interop.LibC;

namespace Tutu.Unix;

/// <summary>
/// A file descriptor wrapper.
///
/// It allows to retrieve raw file descriptor, write to the file descriptor and
/// mainly it closes the file descriptor once dropped.
/// </summary>
/// <param name="Fd"></param>
/// <param name="CloseOnDispose"></param>
internal readonly record struct FileDesc(int Fd, bool CloseOnDispose) : IDisposable
{
    /// <summary>
    /// Creates a file descriptor pointing to the standard input or `/dev/tty`.
    /// </summary>
    /// <returns></returns>
    public static FileDesc TtyFd()
    {
        if (LibC.isatty(LibC.STDIN_FILENO) == 1)
        {
            return new(LibC.STDIN_FILENO, false);
        }

        var path = "/dev/tty".ToPathname();
        var fd = LibC.open(path, LibC.O_RDWR);

        return new FileDesc(fd, true);
    }

    public unsafe uint Read(Span<byte> buffer)
    {
        ssize_t read;
        fixed (void* ptr = buffer)
        {
            read = LibC.read(Fd, ptr, (uint)buffer.Length);
        }

        if (read < 0)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }

        return (uint)read;
    }

    public void Dispose()
    {
        if (CloseOnDispose)
        {
            LibC.close(Fd);
        }
    }

    public static implicit operator int(FileDesc fd) => fd.Fd;
    
    // public static implicit operator nint(FileDesc fd) => fd.Fd;
}
