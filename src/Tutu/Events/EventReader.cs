using System.Runtime.InteropServices;
using NodaTime;
using Tutu.Unix;
using Tutu.Windows;

namespace Tutu.Events;

public static class EventReader
{
    private static Mutex<InternalReader> InternalReader { get; } = new(new(CreateEventSource()));

    private static IEventSource CreateEventSource()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new WindowsEventSource();
        }

        return UnixEventSource.Instance;
    }

    /// <summary>
    /// Checks if there is an [`Event`](enum.Event.html) available.
    /// </summary>
    /// <param name="timeout">Maximum waiting time for event availability.</param>
    /// <returns>true if an <see cref="IEvent"/> is available. otherwise return false.</returns>
    public static bool Poll(Duration timeout)
        => Poll(SystemClock.Instance, timeout);

    public static bool Poll(IClock clock, Duration timeout)
        => PollInternal(clock, timeout, PublicEventFilter.Default);

    /// <summary>
    /// Reads a single <see cref="IEvent"/>.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This function blocks until an <see cref="IEvent"/> is available.
    /// </remarks>
    public static IEvent Read()
    {
        var @event = ReadInternal(PublicEventFilter.Default);
        if (@event is PublicEvent publicEvent)
        {
            return publicEvent.Event;
        }

        // TODO: throw a better exception.
        throw new Exception();
    }


    internal static bool PollInternal<TFilter>(Duration? timeout, TFilter filter)
        where TFilter : IFilter
        => PollInternal(SystemClock.Instance, timeout, filter);

    internal static bool PollInternal<TFilter>(IClock clock, Duration? timeout, TFilter filter)
        where TFilter : IFilter
    {
        Mutex<InternalReader>.ValueAccess? access;
        if (timeout != null)
        {
            var pollTimeout = new PollTimeout(clock, timeout.Value);
            if (!InternalReader.TryLock(pollTimeout.Leftover.GetValueOrDefault(), out access))
            {
                return false;
            }
        }
        else
        {
            access = InternalReader.Lock();
        }

        try
        {
            return access.Value.Poll(timeout, filter);
        }
        finally
        {
            access.Dispose();
        }
    }

    internal static IInternalEvent ReadInternal<TFilter>(TFilter filter)
        where TFilter : IFilter
    {
        using var access = InternalReader.Lock();
        return access.Value.Read(filter);
    }
}