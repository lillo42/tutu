using NodaTime;

namespace Tutu.Events;

public interface IEventSource
{
    IInternalEvent? TryRead(IClock clock, Duration? timeout);
    IInternalEvent? TryRead(Duration? timeout)
        => TryRead(SystemClock.Instance, timeout);
}