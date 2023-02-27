using System.Runtime.InteropServices;
using System.Threading.Channels;
using NodaTime;
using Tutu.Windows;

namespace Tutu.Events;

public class EventStream
{
    private readonly Channel<IEvent> _channel;
    private Task? _consumeTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public EventStream(int capacity)
    {
        _consumeTask = null;
        _channel = Channel.CreateBounded<IEvent>(new BoundedChannelOptions(capacity));
    }


    public static EventStream Default { get; } = new(1_000);

    public ChannelReader<IEvent> Reader => _channel.Reader;

    public void Start(IClock? clock = null) => StartAsync(clock).GetAwaiter().GetResult();

    public void Stop() => StopAsync().GetAwaiter().GetResult();

    public async Task StartAsync(IClock? clock = null)
    {
        await StopAsync().ConfigureAwait(false);
        _cancellationTokenSource = new CancellationTokenSource();
        _consumeTask = ConsumeEvents(clock ?? SystemClock.Instance, _cancellationTokenSource.Token);
    }

    public async Task StopAsync()
    {
        _cancellationTokenSource?.Cancel();
        if (_consumeTask != null)
        {
            await _consumeTask.ConfigureAwait(false);
        }
    }

    private async Task ConsumeEvents(
        IClock clock,
        CancellationToken cancellationToken)
    {
        await Task.Yield();

        while (!cancellationToken.IsCancellationRequested)
        {
            var hasEvent = EventReader.Poll(clock, Duration.FromMilliseconds(10));
            if (hasEvent)
            {
                try
                {
                    var @event = EventReader.Read();
                    await _channel.Writer.WriteAsync(@event, cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}