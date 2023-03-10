using NodaTime;

namespace Tutu.Events;

internal class InternalReader
{
    private readonly IEventSource _source;
    private readonly List<IInternalEvent> _events;
    private readonly List<IInternalEvent> _skippedEvents;

    public InternalReader(IEventSource source)
    {
        _source = source;

        _events = new(32);
        _skippedEvents = new(32);
    }

    public bool Poll<TFilter>(Duration? duration, TFilter filter)
        where TFilter : IFilter
        => Poll(SystemClock.Instance, duration, filter);

    public bool Poll<TFilter>(IClock clock, Duration? duration, TFilter filter)
        where TFilter : IFilter
    {
        if (_events.Any(filter.Eval))
        {
            return true;
        }

        var pollTimeout = new PollTimeout(clock, duration);

        while (true)
        {
            // TODO: try/catch if kind is Interrupted
            var maybeEvent = _source.TryRead(clock, pollTimeout.Leftover);
            if (maybeEvent != null && !filter.Eval(maybeEvent))
            {
                _skippedEvents.Add(maybeEvent);
                maybeEvent = null;
            }

            if (pollTimeout.Elapsed || maybeEvent != null)
            {
                _events.AddRange(_skippedEvents);
                _skippedEvents.Clear();

                if (maybeEvent != null)
                {
                    _events.Insert(0, maybeEvent);
                    return true;
                }

                return false;
            }
        }
    }

    public IInternalEvent Read<TFilter>(TFilter filter)
        where TFilter : IFilter
    {
        while (true)
        {
            for (var index = 0; index < _events.Count; index++)
            {
                var @event = _events[index];
                if (filter.Eval(@event))
                {
                    _events.RemoveAt(index);
                    return @event;
                }
            }

            _ = Poll(null, filter);
        }

    }
}
