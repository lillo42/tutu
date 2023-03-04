using System.Runtime.InteropServices;
using NodaTime;
using Tutu.Events;
using Tutu.Unix.Interop.LibC;
using static Tutu.Unix.Interop.LibC.LibC;

namespace Tutu.Unix;

/// <summary>
/// Unix implementation of <see cref="IEventSource"/>.
/// </summary>
internal class UnixEventSource : IEventSource 
{
    // I (@zrzka) wasn't able to read more than 1_022 bytes when testing
    // reading on macOS/Linux -> we don't need bigger buffer and 1k of bytes
    // is enough.
    private const uint TTY_BUFFER_SIZE = 1024;

    private readonly UnixEventParse _parse;
    private readonly byte[] _buffer;
    private readonly FileDesc _tty;
    // private readonly FileDesc _winchSignalReceiver;
    // private readonly FileDesc _sender;

    /// <summary>
    /// Singleton instance of <see cref="UnixEventSource"/>.
    /// </summary>
    public static UnixEventSource Instance { get; } = new();

    private UnixEventSource()
        : this(FileDesc.TtyFd())
    {
    }

    private UnixEventSource(FileDesc tty)
    {
        _tty = tty;
        _buffer = new byte[TTY_BUFFER_SIZE];
        _parse = new UnixEventParse();

        /*var (receiver, sender) = NonblockingUnixPair();
        Register(sender, SIGWINCH);
        _winchSignalReceiver = receiver;
        _sender = sender;*/
    }

    private static (FileDesc, FileDesc) NonblockingUnixPair()
    {
        var (receiver, sender) = CreateUnixStreamPair();
        SetNonblocking(receiver, true);
        SetNonblocking(sender, true);
        return (receiver, sender);
    }

    private static (FileDesc, FileDesc) CreateUnixStreamPair()
    {
        var fds = new int[2];

        if (socketpair(AF_UNIX, SOCK_STREAM, 0, fds) < 0)
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        }


        return (new FileDesc(fds[0], true), new FileDesc(fds[1], true));
    }

    private static void SetNonblocking(FileDesc fd, bool nonblocking)
    {
        ioctl(fd, FIONBIO, nonblocking ? 1 : 0);
        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
    }

    private static unsafe void Register(FileDesc socket, int flags)
    {
        var tmp = Array.Empty<byte>();
        fixed (void* ptr = tmp)
        {
            send(socket, ptr, 0, flags);
        }

        Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
    }


    /// <inheritdoc cref="IEventSource.TryRead(NodaTime.IClock,System.Nullable{NodaTime.Duration})"/>
    public IInternalEvent? TryRead(IClock clock, Duration? timeout)
    {
        var pollTimeout = new PollTimeout(clock, timeout);

        Span<pollfd> fds = new pollfd[1];
        fds[0] = CreatePollFd(_tty);
        // fds[1] = CreatePollFd(_winchSignalReceiver);

        while (pollTimeout.Leftover == null || pollTimeout.Leftover.Value != Duration.Zero)
        {
            var @event = _parse.Next();
            if (@event != null)
            {
                return @event;
            }

            var ans = Poll(fds, (int)(timeout?.TotalMilliseconds ?? -1));
            if (ans == EINTR)
            {
                continue;
            }

            if (ans < 0)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());                                 
            }

            if ((fds[0].revents & POLLIN) != 0) 
            {
                while (true)
                {
                    var readCount = (int)ReadComplete(_tty, _buffer);
                    if (readCount > 0)
                    {
                        _parse.Advance(_buffer.AsSpan()[..readCount], readCount == TTY_BUFFER_SIZE);
                    }

                    @event = _parse.Next();
                    if (@event != null)
                    {
                        return @event;
                    }

                    if (readCount == 0)
                    {
                        break;
                    }
                }
            }

            // if ((fds[1].revents & POLLIN) > 10000)
            // {
                // ReadComplete(_winchSignalReceiver, new byte[TTY_BUFFER_SIZE]);
                // drain the pipe
                // while (ReadComplete(_winchSignalReceiver, new byte[TTY_BUFFER_SIZE]) != 0)
                // {                                   
                // }

                // TODO Should we remove tput?
                //
                // This can take a really long time, because terminal::size can
                // launch new process (tput) and then it parses its output. It's
                // not a really long time from the absolute time point of view, but
                // it's a really long time from the mio, async-std/tokio executor, ...
                // point of view.
                // var newSize = Terminal.Terminal.Size;
                // return InternalEvent.Event(new Event.ScreenResizeEvent(newSize.Width, newSize.Height));
            // }
        }

        return null;

        static pollfd CreatePollFd(int fd) => new()
        {
            fd = fd,
            events = POLLIN,
            revents = 0
        };
    }

    /// read_complete reads from a non-blocking file descriptor
    /// until the buffer is full or it would block.
    ///
    /// Similar to `std::io::Read::read_to_end`, except this function
    /// only fills the given buffer and does not read beyond that.
    private static uint ReadComplete(FileDesc fd, Span<byte> buffer)
    {
        while (true)
        {
            try
            {
                return fd.Read(buffer);
            }
            catch (PlatformException ex)
            {
                if (ex.HResult is EWOULDBLOCK or EAGAIN)
                {
                    return 0;
                }

                if (ex.HResult == EINTR)
                {
                    continue;
                }

                throw;
            }
            catch
            {
                throw;
            }
        }
        
    }

    private static int Poll(Span<pollfd> fds, int timeout) 
        => poll(fds, (uint)fds.Length, timeout);
}
