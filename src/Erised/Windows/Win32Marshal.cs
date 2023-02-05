using System.Runtime.InteropServices;

namespace Erised;

internal static partial class Windows
{
    private static Exception GetExceptionForWin32Error(int error) 
        => new IOException(Marshal.GetPInvokeErrorMessage(error), MakeHRFromErrorCode(error));

    /// <summary>
    /// If not already an HRESULT, returns an HRESULT for the specified Win32 error code.
    /// </summary>
    private static int MakeHRFromErrorCode(int errorCode)
    {
        // Don't convert it if it is already an HRESULT
        if ((0xFFFF0000 & errorCode) != 0)
        {
            return errorCode;
        }

        return unchecked((int)0x80070000 | errorCode);
    }
}