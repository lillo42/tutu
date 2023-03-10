namespace Tutu.Events;

/// <summary>
/// An internal event.
///
/// Encapsulates publicly available `Event` with additional internal
/// events that shouldn't be publicly available to the crate users.
/// </summary>
internal static class InternalEvent
{
    public static PrimaryDeviceAttributesInternalEvent PrimaryDeviceAttributes { get; } = new();

    public static PublicEvent Event(IEvent @event) => new(@event);

    public static CursorPositionInternalEvent CursorPosition(int column, int row) => new(column, row);

    public static KeyboardEnhancementFlagsInternalEvent KeyboardEnhancementFlags(KeyboardEnhancementFlags flags) =>
        new(flags);
}

/// <summary>
/// An internal event.
/// </summary>
/// <remarks>
/// Encapsulates publicly available `Event` with additional internal
/// events that shouldn't be publicly available to the crate users.
/// </remarks>
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
internal record CursorPositionInternalEvent(int Column, int Row) : IInternalEvent;

/// <summary>
/// The progressive keyboard enhancement flags enabled by the terminal.
/// </summary>
internal record KeyboardEnhancementFlagsInternalEvent(KeyboardEnhancementFlags Flags) : IInternalEvent;

/// <summary>
/// Attributes and architectural class of the terminal.
/// </summary>
internal record PrimaryDeviceAttributesInternalEvent : IInternalEvent;
