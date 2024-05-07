using System.Threading.Channels;
using NodaTime;

namespace Tutu.Events;

/// <summary>
/// The event stream.
/// </summary>
public sealed class EventStream
{
    private readonly Channel<IEvent> _channel;
    private Task? _consumeTask;
    private CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStream"/> class.
    /// </summary>
    /// <param name="capacity">The <see cref="Channel"/> capacity.</param>
    public EventStream(int capacity)
    {
        _consumeTask = null;
        _channel = Channel.CreateBounded<IEvent>(new BoundedChannelOptions(capacity));
    }

    /// <summary>
    /// The default implementation of <see cref="EventStream"/>.
    /// </summary>
    /// <remarks>
    /// The default implementation has a capacity of 1,000.
    /// </remarks>
    public static EventStream Default { get; } = new(1_000);

    /// <summary>
    /// The <see cref="ChannelReader{T}"/> for the <see cref="IEvent"/>.
    /// </summary>
    public ChannelReader<IEvent> Reader => _channel.Reader;

    /// <summary>
    /// Starts the <see cref="EventStream"/>.
    /// </summary>
    /// <param name="clock">The <see cref="IClock"/>.</param>
    public void Start(IClock? clock = null) => StartAsync(clock).GetAwaiter().GetResult();

    /// <summary>
    /// Stops the <see cref="EventStream"/>.
    /// </summary>
    public void Stop() => StopAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Starts the <see cref="EventStream"/>.
    /// </summary>
    /// <param name="clock">The <see cref="IClock"/>.</param>
    public async Task StartAsync(IClock? clock = null)
    {
        await StopAsync().ConfigureAwait(false);

        _cancellationTokenSource = new CancellationTokenSource();
        _consumeTask = ConsumeEvents(clock ?? SystemClock.Instance, _cancellationTokenSource.Token);
    }

    /// <summary>
    /// Stops the <see cref="EventStream"/>.
    /// </summary>
    public async Task StopAsync()
    {
        if (_cancellationTokenSource != null)
        {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

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
            var hasEvent = SystemEventReader.Poll(clock, Duration.FromMilliseconds(10));
            if (hasEvent)
            {
                try
                {
                    var @event = SystemEventReader.Read();
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
