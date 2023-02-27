using System.Runtime.InteropServices;
using System.Threading.Channels;
using NodaTime;
using Tutu.Windows;

namespace Tutu.Events;

public class EventStream
{
    private static readonly object Lock = new();

    private readonly Channel<IEvent> _channel;

    private CancellationTokenSource? _cancellationTokenSource;

    private EventStream()
    {
        _channel = Channel.CreateBounded<IEvent>(new BoundedChannelOptions(1_000));
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

    public void Start(Duration? timeout = null, IClock? clock = null)
    {
        Stop();
        _cancellationTokenSource = new CancellationTokenSource();
        ConsumeEvents(clock ?? SystemClock.Instance, _cancellationTokenSource.Token);
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void ConsumeEvents(
        IClock clock,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var hasEvent = EventReader.PollInternal(clock, Duration.FromSeconds(1), EventFilter.Default);
            if (hasEvent)
            {
                var @event = EventReader.ReadInternal(EventFilter.Default);
                if (@event is InternalEvent.PublicEvent publicEvent)
                {
                    _channel.Writer.TryWrite(publicEvent.Event);
                }
            }
        }
    }
}