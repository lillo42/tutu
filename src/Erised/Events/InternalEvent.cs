namespace Erised.Events;

/// <summary>
/// An internal event.
///
/// Encapsulates publicly available `Event` with additional internal
/// events that shouldn't be publicly available to the crate users.
/// </summary>
internal static class InternalEvent
{
    public static IInternalEvent PrimaryDeviceAttributes { get; } = new PrimaryDeviceAttributesInternalEvent();

    public static IInternalEvent Event(IEvent @event) => new PublicEvent(@event);

    public static IInternalEvent CursorPosition(ushort column, ushort row) => new CursorPositionInternalEvent(column, row);

    public static IInternalEvent KeyboardEnhancementFlags(KeyboardEnhancementFlags flags) =>
        new KeyboardEnhancementFlagsInternalEvent(flags);
}

/// <summary>
/// An internal event.
///
/// Encapsulates publicly available `Event` with additional internal
/// events that shouldn't be publicly available to the crate users.
/// </summary>
internal interface IInternalEvent
{
}

/// <summary>
/// An event.
/// </summary>
/// <param name="Event"></param>
internal record PublicEvent(IEvent Event) : IInternalEvent;

/// <summary>
/// A cursor position
/// </summary>
/// <param name="Column"></param>
/// <param name="Row"></param>
internal record CursorPositionInternalEvent(ushort Column, ushort Row) : IInternalEvent;

/// <summary>
/// The progressive keyboard enhancement flags enabled by the terminal.
/// </summary>
internal record KeyboardEnhancementFlagsInternalEvent(KeyboardEnhancementFlags Flags) : IInternalEvent;

/// <summary>
/// Attributes and architectural class of the terminal.
/// </summary>
internal record PrimaryDeviceAttributesInternalEvent : IInternalEvent;