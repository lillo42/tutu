using System.Runtime.InteropServices;

namespace Erised.Events;

/// <summary>
/// Interface for filtering an `InternalEvent`.
/// </summary>
internal interface IFilter
{
    /// <summary>
    /// Returns whether the given event fulfills the filter.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    bool Eval(IInternalEvent @event);
}

internal record EventFilter : IFilter
{
    public bool Eval(IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return true;
        }

        return @event is PublicEvent;
    }
}

internal record CursorPositionFilter : IFilter
{
    public bool Eval(IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        return @event is CursorPositionInternalEvent;
    }
}

internal record KeyboardEnhancementFlagsFilter : IFilter
{
    public bool Eval(IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        return @event is KeyboardEnhancementFlagsInternalEvent;
    }
}

internal record PrimaryDeviceAttributesFilter : IFilter
{
    public bool Eval(IInternalEvent @event)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        return @event is PrimaryDeviceAttributesInternalEvent;
    }
}

internal record InternalEventFilter : IFilter
{
    public bool Eval(IInternalEvent @event) => true;
}