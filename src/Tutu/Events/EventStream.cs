using System.Runtime.InteropServices;
using System.Threading.Channels;
using NodaTime;

namespace Tutu.Events;

public class EventStream
{
    private static readonly object Lock = new();

    private readonly IEventSource _source;
    private readonly Channel<IEvent> _channel;

    private CancellationTokenSource? _cancellationTokenSource;

    private EventStream()
    {
        _channel = Channel.CreateBounded<IEvent>(new BoundedChannelOptions(1_000));
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // _windowsEventStream = new Windows.WindowsEventStream(1_000);
        }
        else
        {
            _source = new Unix.Events.Unix.EventSource(Unix.Unix.FileDesc.TtyFd());
        }
    }

    private static EventStream? _default;

    public static EventStream Default
    {
        get
        {
            if (_default == null)
            {
                lock (Lock)
                {
                    if (_default == null)
                    {
                        _default = new EventStream();
                    }
                }
            }

            return _default;
        }
    }

    public ChannelReader<IEvent> Reader => _channel.Reader;

    public void Start(Duration? timeout = null,
        Action? onTimeout = null,
        IClock? clock = null)
    {
        Stop();
        _cancellationTokenSource = new CancellationTokenSource();
        ConsumeEvents(clock ?? SystemClock.Instance, onTimeout, _cancellationTokenSource.Token);
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
    }

    public IEvent? Read(
        Duration? timeout = null,
        Action? onTimeout = null,
        IClock? clock = null)
    {
        return null;
    }

    private void ConsumeEvents(
        IClock clock,
        Action? onTimeout,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var hasEvent = EventReader.PollInternal(clock, null, EventFilter.Default);
            if (hasEvent)
            {
                var @event = EventReader.ReadInternal(EventFilter.Default);
                if (@event is InternalEvent.PublicEvent publicEvent)
                {
                    _channel.Writer.TryWrite(publicEvent.Event);
                }
            }
            else if(!cancellationToken.IsCancellationRequested)
            {
                onTimeout?.Invoke();
            }
        }
    }
}