using System.Runtime.InteropServices;

namespace Tutu.Events;

/// <summary>
/// Interface for filtering an `InternalEvent`.
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Returns whether the given event fulfills the filter.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    bool Eval(InternalEvent.IInternalEvent @event);
}

public record EventFilter : IFilter
{
    public static EventFilter Default { get; } = new();

    public bool Eval(InternalEvent.IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return true;
        }

        return @event is InternalEvent.PublicEvent;
    }
}

public record CursorPositionFilter : IFilter
{
    public static CursorPositionFilter Default { get; } = new();

    public bool Eval(InternalEvent.IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        return @event is InternalEvent.CursorPositionInternalEvent;
    }
}

public record KeyboardEnhancementFlagsFilter : IFilter
{
    public static KeyboardEnhancementFlagsFilter Default { get; } = new();

    public bool Eval(InternalEvent.IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        return @event is InternalEvent.KeyboardEnhancementFlagsInternalEvent;
    }
}

public record PrimaryDeviceAttributesFilter : IFilter
{
    public static PrimaryDeviceAttributesFilter Default { get; } = new();

    public bool Eval(InternalEvent.IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        return @event is InternalEvent.PrimaryDeviceAttributesInternalEvent;
    }
}

public record InternalEventFilter : IFilter
{
    public static InternalEventFilter Default { get; } = new();
    public bool Eval(InternalEvent.IInternalEvent @event) => true;
}