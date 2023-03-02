using NodaTime;
using Tmds.Linux;
using Tutu.Events;
using static Tmds.Linux.LibC;

namespace Tutu.Unix;

/// <summary>
/// Unix implementation of <see cref="IEventSource"/>.
/// </summary>
public class UnixEventSource : IEventSource
{
    // I (@zrzka) wasn't able to read more than 1_022 bytes when testing
    // reading on macOS/Linux -> we don't need bigger buffer and 1k of bytes
    // is enough.
    private const uint TTY_BUFFER_SIZE = 1024;

    private readonly UnixEventParse _parse;
    private readonly byte[] _buffer = new byte[TTY_BUFFER_SIZE];
    private readonly FileDesc _tty;
    private readonly FileDesc _winchSignalReceiver;

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

        var (receiver, sender) = NonblockingUnixPair();
        Register(sender, SIGWINCH);
        _winchSignalReceiver = receiver;
    }

    private static (FileDesc, FileDesc) NonblockingUnixPair()
    {
        var (receiver, sender) = CreateUnixStreamPair();
        SetNonblocking(receiver, true);
        SetNonblocking(sender, true);
        return (receiver, sender);
    }

    private static unsafe (FileDesc, FileDesc) CreateUnixStreamPair()
    {
        var fds = new int[2];

        fixed (int* ptr = fds)
        {
            socketpair(LibC.AF_UNIX, LibC.SOCK_STREAM | LibC.SOCK_CLOEXEC, 0, ptr);
        }

        PlatformException.Throw();

        return (new FileDesc(fds[0], true), new FileDesc(fds[1], true));
    }

    private static void SetNonblocking(FileDesc fd, bool nonblocking)
    {
        ioctl(fd, FIONBIO, nonblocking ? 1 : 0);
        PlatformException.Throw();
    }

    private static unsafe void Register(FileDesc socket, int flags)
    {
        var tmp = new byte[0];
        fixed (void* ptr = tmp)
        {
            send(socket, ptr, 0, flags);
        }

        PlatformException.Throw();
    }


    public unsafe IInternalEvent? TryRead(IClock clock, Duration? timeout)
    {
        var pollTimeout = new PollTimeout(clock, timeout);

        Span<pollfd> fds = stackalloc pollfd[2];
        fds[0] = CreatePollFd(_tty);
        fds[1] = CreatePollFd(_winchSignalReceiver);

        while (pollTimeout.Leftover == null || pollTimeout.Leftover.Value != Duration.Zero)
        {
            var @event = _parse.Next();
            if (@event != null)
            {
                return @event;
            }

            var ans = Poll(fds, (int)(timeout?.TotalMilliseconds ?? 0));
            if (ans == EINTR)
            {
                continue;
            }

            if (ans < 0)
            {
                PlatformException.Throw();
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

            if ((fds[1].revents & POLLIN) != 0)
            {
                // drain the pipe
                while (ReadComplete(_winchSignalReceiver, new byte[TTY_BUFFER_SIZE]) != 0)
                {
                }

                // TODO Should we remove tput?
                //
                // This can take a really long time, because terminal::size can
                // launch new process (tput) and then it parses its output. It's
                // not a really long time from the absolute time point of view, but
                // it's a really long time from the mio, async-std/tokio executor, ...
                // point of view.
                var newSize = Terminal.Terminal.Size;
                return InternalEvent.Event(new Event.ScreenResizeEvent(newSize.Width, newSize.Height));
            }
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
                if (ex.HResult == EWOULDBLOCK || ex.HResult == EAGAIN)
                {
                    return 0;
                }

                if (ex.HResult == EINTR)
                {
                    continue;
                }

                throw;
            }
        }
    }

    private static unsafe int Poll(Span<pollfd> fds, int timeout)
    {
        fixed (pollfd* ptr = fds)
        {
            poll(ptr, (uint)fds.Length, timeout);
        }

        return errno;
    }
}