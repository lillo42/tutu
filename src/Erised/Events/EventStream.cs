using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Threading.Tasks.Dataflow;
using NodaTime;

namespace Erised.Events;

public class EventStream
{
    private readonly Windows.WindowsEventStream? _windowsEventStream;
    private readonly Unix.UnixEventStream? _unixEventStream;
    private Task? _consumeEvent;
    private CancellationTokenSource? _source;

    private EventStream(int capacity)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            _windowsEventStream = new Windows.WindowsEventStream(capacity);
        }
        else
        {
            _unixEventStream = new Unix.UnixEventStream(capacity);
        }
    }

    private static object _lock = new();
    private static EventStream? _default;
    private static int _capacity = 1_000;
    public static int Capacity
    {
        get => _capacity;
        set
        {
            if (_default != null)
            {
                throw new InvalidOperationException("Cannot change capacity after default stream has been created.");
            }
            _capacity = value;
        }
    }

    public static EventStream Default
    {
        get
        {
            if (_default == null)
            {
                lock (_lock)
                {
                    if (_default == null)
                    {
                        _default = new EventStream(Capacity);
                    }
                }
            }

            return new EventStream(Capacity);
        }
    }

    public ChannelReader<IEvent> Reader
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return _windowsEventStream!.Reader;
            }

            return _unixEventStream!.Reader;
        }
    }

    public void Start(Duration? timeout = null,
        Action? onTimeout = null,
        IClock? clock = null)
    {
        Stop();
        _source = new CancellationTokenSource();
        ConsumeEvents(clock, timeout, onTimeout, _source.Token);
    }

    public void Stop()
    {
        _source?.Cancel();
        _consumeEvent?.Wait();
    }

    public IEvent? Read(
        Duration? timeout = null,
        Action? onTimeout = null,
        IClock? clock = null)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return _windowsEventStream!.Read(clock ?? SystemClock.Instance, timeout, onTimeout);
        }

        return null;
    }

    private void ConsumeEvents(
        IClock? clock,
        Duration? timeout,
        Action? onTimeout,
        CancellationToken cancellationToken)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            _consumeEvent = _windowsEventStream!
                .ConsumeConsoleEventAsync(
                    clock ?? SystemClock.Instance,
                    timeout,
                    onTimeout,
                    cancellationToken);
        }
    }
}