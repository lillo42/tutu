using System.Runtime.InteropServices;
using Tutu.Unix.Interop.LibC;
using static Tutu.Unix.Interop.LibC.LibC;

namespace Tutu.Unix;

internal class Pipe
{
    private static readonly HashSet<WakeFd> s_register = new();

    public static unsafe void Register(FileDesc fd, int signal)
    {
        ssize_t ret = 0;
        var tmp = Array.Empty<byte>();
        fixed (void* ptr = tmp)
        {
            ret = send(fd, ptr, 0, MSG_DONTWAIT);
        }

        WakeFd wake;
        if (ret == 0)
        {
            wake = new WakeFd(fd, WakeMethod.Send);
        }
        else
        {
            wake = new WakeFd(fd, WakeMethod.Write);
            wake.SetFlags();
        }

        if (s_register.Add(wake))
        {
            Register(signal, _ => wake.Wake());
        }
    }

    private static void Register(int signal, SignalHandler handler)
    {
        LibC.signal(signal, handler);
    }
}

internal record WakeFd(int Fd, WakeMethod Method)
{
    public void SetFlags()
    {
        var flags = fcntl(Fd, F_GETFL, 0);
        if (flags < 0)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }

        flags |= O_NONBLOCK | O_CLOEXEC;
        if (fcntl(Fd, F_SETFL, flags) == -1)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }

    public unsafe void Wake()
    {
        // This writes some data into the pipe.
        //
        // There are two tricks:
        // * First, the crazy cast. The first part turns reference into pointer. The second part
        //   turns pointer to u8 into a pointer to void, which is what write requires.
        // * Second, we ignore errors, on purpose. We don't have any means to handling them. The
        //   two conceivable errors are EBADFD, if someone passes a non-existent file descriptor or
        //   if it is closed. The second is EAGAIN, in which case the pipe is full ‒ there were
        //   many signals, but the reader didn't have time to read the data yet. It'll still get
        //   woken up, so not fitting another letter in it is fine.
        var data = "X"u8.ToArray();
        fixed (void* ptr = data)
        {
            if (Method == WakeMethod.Write)
            {
                write(Fd, ptr, data.Length);
            }
            else if (Method == WakeMethod.Send)
            {
                send(Fd, ptr, 1, MSG_DONTWAIT);
            }
        }
    }
}

internal enum WakeMethod
{
    Send,
    Write
}
