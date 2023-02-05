namespace Erised.Events;

/// <summary>
/// Represents an event.
/// </summary>
public static class Event
{
    /// <summary>
    /// The terminal gained focus.
    /// </summary>
    public static FocusGainedEvent FocusGained { get; } = new();

    /// <summary>
    /// The terminal lost focus.
    /// </summary>
    public static FocusLostEvent FocusLost { get; } = new();

    /// <summary>
    /// A single key event with additional pressed modifiers.
    /// </summary>
    /// <param name="event">The <see cref="Events.KeyEvent"/>.</param>
    /// <returns>New instance of <see cref="KeyEvent"/>.</returns>
    public static KeyEvent Key(Events.KeyEvent @event) => new(@event);

    /// <summary>
    /// A single mouse event with additional pressed modifiers.
    /// </summary>
    /// <param name="event">The <see cref="Erised.Events.MouseEvent"/>.</param>
    /// <returns>New instance of <see cref="Mouse"/>.</returns>
    public static MouseEvent Mouse(Events.MouseEvent @event) => new MouseEvent(@event);

    /// <summary>
    /// A string that was pasted into the terminal.
    /// </summary>
    /// <param name="text">The pasted text.</param>
    /// <returns>New instance of <see cref="TextPasted"/>.</returns>
    public static IEvent Pasted(string text) => new TextPasted(text);

    /// <summary>
    ///  An resize event with new dimensions after resize (columns, rows).
    /// </summary>
    /// <remarks>
    /// that resize events can occur in batches.
    /// </remarks>
    /// <param name="column">The new column</param>
    /// <param name="row">The new row</param>
    public static IEvent Resize(ushort column, ushort row) => new ScreenResize(column, row);

    /// <summary>
    /// The terminal gained focus
    /// </summary>
    public record FocusGainedEvent : IEvent;

    /// <summary>
    /// The terminal lost focus
    /// </summary>
    public record FocusLostEvent : IEvent;

    /// <summary>
    /// A single key event with additional pressed modifiers.
    /// </summary>
    /// <param name="Event">The <see cref="Events.KeyEvent"/>.</param>
    public record KeyEvent(Events.KeyEvent Event) : IEvent;

    /// <summary>
    /// A single mouse event with additional pressed modifiers.
    /// </summary>
    /// <param name="Event">The <see cref="Erised.Events.MouseEvent"/>.</param>
    public record MouseEvent(Events.MouseEvent Event) : IEvent;

    /// <summary>
    /// A string that was pasted into the terminal.
    /// </summary>
    /// <param name="Text"></param>
    public record TextPasted(string Text) : IEvent;

    /// <summary>
    ///  An resize event with new dimensions after resize (columns, rows).
    /// </summary>
    /// <remarks>
    /// that resize events can occur in batches.
    /// </remarks>
    /// <param name="Column">The new column</param>
    /// <param name="Row">The new row</param>
    public record ScreenResize(ushort Column, ushort Row) : IEvent;
}

/// <summary>
/// Represents an event.
/// </summary>
public interface IEvent
{
}