using System.Runtime.InteropServices;

namespace Tutu.Windows.Interop.Kernel32;

internal static partial class Kernel32
{
    public const uint WAIT_OBJECT_0 = 0x00000000;
    public const uint WAIT_ABANDONED = 0x00000080;
    public const uint WAIT_TIMEOUT = 0x00000102;
    public const uint WAIT_FAILED = 0xFFFFFFFF;

    [LibraryImport(LibraryName, SetLastError = true)]
    public static partial uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);
}