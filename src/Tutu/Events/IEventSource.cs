using NodaTime;

namespace Tutu.Events;

/// <summary>
/// The event source.
/// </summary>
internal interface IEventSource
{
    /// <summary>
    /// Try to read an <see cref="IInternalEvent"/>.
    /// </summary>
    /// <param name="clock">The <see cref="IClock"/>.</param>
    /// <param name="timeout">Maximum waiting time for event availability.</param>
    /// <returns>The <see cref="IInternalEvent"/> if could read; otherwise return <see langword="null"/>.</returns>
    IInternalEvent? TryRead(IClock clock, Duration? timeout);

    /// <summary>
    /// Try to read an <see cref="IInternalEvent"/>.
    /// </summary>
    /// <param name="timeout">Maximum waiting time for event availability.</param>
    /// <returns>The <see cref="IInternalEvent"/> if could read; otherwise return <see langword="null"/>.</returns>
    IInternalEvent? TryRead(Duration? timeout)
        => TryRead(SystemClock.Instance, timeout);
}
