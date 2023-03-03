using System.Runtime.InteropServices;
using Tutu.Exceptions;
using static Tmds.Linux.LibC;

namespace Tutu.Unix;

/// <summary>
/// Exception thrown when a platform call fails.
/// </summary>
public class PlatformException : TutuException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformException"/> class.
    /// </summary>
    /// <param name="errno"></param>
    public PlatformException(int errno)
        : base(GetErrorMessage(errno))
    {
        HResult = errno;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlatformException"/> class.
    /// </summary>
    public PlatformException()
        : this(errno)
    {
    }

    private static unsafe string? GetErrorMessage(int errno)
    {
        const int bufferLength = 1024;
        var buffer = stackalloc byte[bufferLength];

        var rv = strerror_r(errno, buffer, bufferLength);

        return rv == 0 ? Marshal.PtrToStringAnsi((nint)buffer) : $"errno {errno}";
    }

    internal static void Throw()
    {
        if (errno < 0)
        {
            throw new PlatformException();
        }
    }
}
