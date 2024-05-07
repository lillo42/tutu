using System.Diagnostics;
using System.Runtime.InteropServices;
using NodaTime;
using Tutu.Unix;
using Tutu.Windows;

namespace Tutu.Events;

/// <summary>
/// The event reader
/// </summary>
public interface IEventReader
{
    /// <summary>
    /// Checks if there is an <see cref="IEvent"/>.
    /// </summary>
    /// <param name="timeout">Maximum waiting time for event availability.</param>
    /// <returns><see langword="true"/> if an <see cref="IEvent"/> is available; otherwise return <see langword="true"/>.</returns>
    bool Poll(Duration? timeout = null);

    /// <summary>
    /// Checks if there is an <see cref="IEvent"/>.
    /// </summary>
    /// <param name="clock">The <see cref="IClock"/>.</param>
    /// <param name="timeout">Maximum waiting time for event availability.</param>
    /// <returns><see langword="true"/> if an <see cref="IEvent"/> is available; otherwise return <see langword="true"/>.</returns>
    bool Poll(IClock clock, Duration? timeout = null);

    /// <summary>
    /// Reads a single <see cref="IEvent"/>.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This function blocks until an <see cref="IEvent"/> is available.
    /// </remarks>
    IEvent Read();
}

/// <summary>
/// The default implementation of <see cref="IEventReader"/>
/// </summary>
internal sealed class InternalSystemEventReader : IEventReader
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


    /// <inheritdoc cref="IEventReader.Poll(System.Nullable{NodaTime.Duration})"/>
    public bool Poll(Duration? timeout = null)
        => Poll(SystemClock.Instance, timeout);


    /// <inheritdoc cref="IEventReader.Poll(NodaTime.IClock, System.Nullable{NodaTime.Duration})" />
    public bool Poll(IClock clock, Duration? timeout = null) => PollInternal(clock, timeout, PublicEventFilter.Default);

    /// <see cref="IEventReader.Read"/>
    public IEvent Read()
    {
        var @event = ReadInternal(PublicEventFilter.Default);
        if (@event is PublicEvent publicEvent)
        {
            return publicEvent.Event;
        }

        throw new UnreachableException("Invalid parse & filter code");
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

/// <summary>
/// The static event reader.
/// </summary>
public static class SystemEventReader
{
    /// <summary>
    /// The singleton instance of <see cref="IEventReader"/>.
    /// </summary>
    public static IEventReader Instance { get; } = new InternalSystemEventReader();


    /// <inheritdoc cref="IEventReader.Poll(System.Nullable{NodaTime.Duration})"/>
    public static bool Poll(Duration? timeout = null) => Instance.Poll(timeout);

    /// <inheritdoc cref="IEventReader.Poll(NodaTime.IClock, System.Nullable{NodaTime.Duration})" />
    public static bool Poll(IClock clock, Duration? timeout = null)
        => Instance.Poll(clock, timeout);

    /// <see cref="IEventReader.Read"/>
    public static IEvent Read()
        => Instance.Read();
}
