using System.Runtime.InteropServices;
using NodaTime;
using Tutu.Windows.Exceptions;
using static Tutu.Windows.Interop.Kernel32.Kernel32;

namespace Tutu.Windows;

internal class WindowsApiPoll
{
    private const uint Infinite = 0xFFFFFFFF;
    public bool Poll(Duration? timeout)
    {
        var milliseconds = timeout?.ToTimeSpan().TotalMilliseconds ?? Infinite;

        var handle = Handle.Create(HandleType.CurrentInput);

        var ouput = WaitForSingleObject(handle, Convert.ToUInt32(milliseconds));

        if (ouput == WAIT_OBJECT_0)
        {
            return true;
        }

        if (ouput is WAIT_TIMEOUT or WAIT_ABANDONED)
        {
            return false;
        }

        if (ouput == WAIT_OBJECT_0 + 1)
        {
            throw new InterruptedPollException("Poll operation was woken up by Waker.");
        }

        if (ouput == WAIT_FAILED)
        {
            throw new FailToWaitPollException(Marshal.GetLastPInvokeErrorMessage());
        }
        
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        return false;
    }
}