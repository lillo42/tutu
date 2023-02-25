namespace Tutu.Events;

/// <summary>
/// An internal event.
///
/// Encapsulates publicly available `Event` with additional internal
/// events that shouldn't be publicly available to the crate users.
/// </summary>
public static class InternalEvent
{
    public static PrimaryDeviceAttributesInternalEvent PrimaryDeviceAttributes { get; } = new();

    public static PublicEvent Event(IEvent @event) => new(@event);

    public static CursorPositionInternalEvent CursorPosition(ushort column, ushort row) => new(column, row);

    public static KeyboardEnhancementFlagsInternalEvent KeyboardEnhancementFlags(KeyboardEnhancementFlags flags) =>
        new(flags);

    /// <summary>
    /// An internal event.
    /// </summary>
    /// <remarks>
    /// Encapsulates publicly available `Event` with additional internal
    /// events that shouldn't be publicly available to the crate users.
    /// </remarks>
    public interface IInternalEvent
    {
    }

    /// <summary>
    /// An event.
    /// </summary>
    /// <param name="Event"></param>
     public record PublicEvent(IEvent Event) : IInternalEvent;

    /// <summary>
    /// A cursor position
    /// </summary>
    /// <param name="Column"></param>
    /// <param name="Row"></param>
    public record CursorPositionInternalEvent(ushort Column, ushort Row) : IInternalEvent;

    /// <summary>
    /// The progressive keyboard enhancement flags enabled by the terminal.
    /// </summary>
    public record KeyboardEnhancementFlagsInternalEvent(KeyboardEnhancementFlags Flags) : IInternalEvent;

    /// <summary>
    /// Attributes and architectural class of the terminal.
    /// </summary>
    public record PrimaryDeviceAttributesInternalEvent : IInternalEvent;
}