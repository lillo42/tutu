using System.Runtime.InteropServices;
using Tmds.Linux;
using static Tmds.Linux.LibC;

namespace Erised;

public class PlatformException : Exception
{
    public PlatformException(int errno) :
        base(GetErrorMessage(errno))
    {
        HResult = errno;
    }

    public PlatformException() :
        this(LibC.errno)
    {
    }

    private unsafe static string? GetErrorMessage(int errno)
    {
        const int bufferLength = 1024;
        var buffer = stackalloc byte[bufferLength];

        var rv = strerror_r(errno, buffer, bufferLength);

        return rv == 0 ? Marshal.PtrToStringAnsi((nint)buffer) : $"errno {errno}";
    }

    public static void Throw() => throw new PlatformException();
}