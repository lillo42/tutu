using System.Runtime.InteropServices;
using System.Threading.Channels;
using NodaTime;

namespace Erised.Events;

public class EventStream
{
    private readonly Channel<IEvent> _channel;
    private readonly Windows.WindowsEventStream _windowsEventStream;
    private Task? _consumeEvent;
    private CancellationTokenSource? _source;

    private EventStream(int capacity)
    {
        _windowsEventStream = new Windows.WindowsEventStream();
        
        _channel = Channel.CreateBounded<IEvent>(new BoundedChannelOptions(capacity)
        {
            SingleReader = false,
            SingleWriter = true,
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });
    }

    public static EventStream Default { get; } = new(1_000);

    public static EventStream Create(int capacity) => new(capacity);
    
    public ChannelReader<IEvent> Reader => _channel.Reader;

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
            return _windowsEventStream.Read(clock ?? SystemClock.Instance, timeout, onTimeout);
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
            _consumeEvent = _windowsEventStream
                .ConsumeConsoleEventAsync(_channel.Writer, 
                    clock ?? SystemClock.Instance,
                    timeout,
                    onTimeout, 
                    cancellationToken);
        }
    }
}