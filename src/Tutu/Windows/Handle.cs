using System.Runtime.InteropServices;
using Tutu.Windows.Interop.Kernel32;
using static Tutu.Windows.Interop.Consts;
using static Tutu.Windows.Interop.Kernel32.Kernel32;

namespace Tutu.Windows;

/// <summary>
/// This abstracts away some WinAPI calls to set and get some console handles.
/// </summary>
/// <remarks>
/// It wraps WinAPI's <see href="https://learn.microsoft.com/en-us/windows/win32/winprog/windows-data-types#handle">HANDLE</see> type.
/// </remarks>
internal readonly struct Handle
{
    public Handle(Inner inner)
    {
        Inner = inner;
    }

    public Inner Inner { get; }

    public nint Value => Inner.Handler;

    /// <summary>
    /// Initialize a new handle from a raw handle.
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    public static Handle Create(nint handle)
        => new(new(handle, true));

    /// <summary>
    /// Create a new handle based on <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The <see cref="HandleType"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Handle Create(HandleType type) => type switch
    {
        HandleType.OutputHandle => OutputHandler(),
        HandleType.InputHandle => InputHandle(),
        HandleType.CurrentOutput => CurrentOutHandle(),
        HandleType.CurrentInput => CurrentInHandle(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    /// <summary>
    /// Get the handle of the input screen buffer.
    /// </summary>
    /// <returns>On success this function return the <see cref="Handle"/> to <see cref="Kernel32.STD_OUTPUT_HANDLE"/>.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/getstdhandle">GetStdHandle</see> called with <see cref="Kernel32.STD_OUTPUT_HANDLE"/>. 
    /// </remarks>
    private static Handle OutputHandler()
    {
        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        return new(new(handle, false));
    }

    /// <summary>
    /// Get the handle of the standard output.
    /// </summary>
    /// <returns>On success this function return the <see cref="Handle"/> to <see cref="Kernel32.STD_OUTPUT_HANDLE"/>.</returns>
    /// <remarks>
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/console/getstdhandle">GetStdHandle</see> called with <see cref="Kernel32.STD_INPUT_HANDLE"/>. 
    /// </remarks>
    private static Handle InputHandle()
    {
        var handle = GetStdHandle(STD_INPUT_HANDLE);
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        return new(new(handle, false));
    }

    /// <summary>
    /// Get the handle of the active screen buffer.
    /// When using multiple screen buffers this will always point to the to the current screen output buffer.
    /// </summary>
    /// <returns>the handle of the active screen buffer.</returns>
    /// <remarks>
    /// This function uses `CONOUT$` to create a file handle to the current output buffer.
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew">CreateFileW</see>. 
    /// </remarks>
    private static Handle CurrentOutHandle()
    {
        var handle = CreateFileW(
            "CONOUT$",
            GENERIC_READ | GENERIC_WRITE,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            nint.Zero,
            OPEN_EXISTING,
            0,
            nint.Zero
        );

        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        return new(new(handle, true));
    }

    /// <summary>
    /// Get the handle of the active screen buffer.
    /// When using multiple screen buffers this will always point to the to the current screen output buffer.
    /// </summary>
    /// <returns>the handle of the active screen buffer.</returns>
    /// <remarks>
    /// This function uses `CONIN$` to create a file handle to the current output buffer.
    /// This wraps <see href="https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew">CreateFileW</see>. 
    /// </remarks>
    private static Handle CurrentInHandle()
    {
        var handle = CreateFileW(
            "CONIN$",
            GENERIC_READ | GENERIC_WRITE,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            nint.Zero,
            OPEN_EXISTING,
            0,
            nint.Zero
        );

        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        return new(new(handle, true));
    }

    public static implicit operator nint(Handle handle)
        => handle.Inner.Handler;
}

/// <summary>
/// Inner structure for closing a handle on <see cref="IDisposable"/>.
/// </summary>
/// <remarks>
/// The second parameter indicates if the HANDLE is exclusively owned or not.
/// A non-exclusive handle can be created using for example
/// <see cref="Handle.InputHandle"/> or <see cref="Handle.OutputHandler"/>, which corresponds to
/// stdin and stdout respectively.
/// </remarks>
internal readonly struct Inner : IDisposable
{
    public Inner(nint handler, bool isExclusive)
    {
        Handler = handler;
        IsExclusive = isExclusive;
    }

    public nint Handler { get; }
    public bool IsExclusive { get; }

    public void Dispose()
    {
        if (IsExclusive)
        {
            CloseHandle(Handler);
        }
    }
}

/// <summary>
/// The standard handles of a process.
/// </summary>
/// <remarks>
/// See <see href="https://docs.microsoft.com/en-us/windows/console/console-handles">The Windows documentation on console handles</see> for more information.
/// </remarks>
internal enum HandleType
{
    /// <summary>
    /// The process' standard output.
    /// </summary>
    OutputHandle,

    /// <summary>
    /// The process' standard input.
    /// </summary>
    InputHandle,

    /// <summary>
    /// The process' active console screen buffer, `CONOUT$`.
    /// </summary>
    CurrentOutput,

    /// <summary>
    /// The process' console input buffer, `CONIN$`.
    /// </summary>
    CurrentInput
}