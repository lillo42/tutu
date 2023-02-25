using NodaTime;

namespace Tutu.Events;

public interface IEventSource
{
    InternalEvent.IInternalEvent? TryRead(IClock clock, Duration? timeout);
    InternalEvent.IInternalEvent? TryRead(Duration? timeout)
        => TryRead(SystemClock.Instance, timeout);
}