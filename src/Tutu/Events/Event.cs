namespace Tutu.Events;

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
    /// <returns>New instance of <see cref="KeyEventEvent"/>.</returns>
    public static KeyEventEvent Key(Events.KeyEvent @event) => new(@event);

    /// <summary>
    /// A single mouse event with additional pressed modifiers.
    /// </summary>
    /// <param name="event">The <see cref="Events.MouseEvent"/>.</param>
    /// <returns>New instance of <see cref="Mouse"/>.</returns>
    public static MouseEventEvent Mouse(Events.MouseEvent @event) => new(@event);

    /// <summary>
    /// A string that was pasted into the terminal.
    /// </summary>
    /// <param name="text">The pasted text.</param>
    /// <returns>New instance of <see cref="TextPastedEvent"/>.</returns>
    public static TextPastedEvent Pasted(string text) => new(text);

    /// <summary>
    ///  An resize event with new dimensions after resize (columns, rows).
    /// </summary>
    /// <remarks>
    /// that resize events can occur in batches.
    /// </remarks>
    /// <param name="column">The new column</param>
    /// <param name="row">The new row</param>
    public static ScreenResizeEvent Resize(ushort column, ushort row) => new(column, row);

    /// <summary>
    /// The terminal gained focus
    /// </summary>
    public sealed record FocusGainedEvent : IEvent;

    /// <summary>
    /// The terminal lost focus
    /// </summary>
    public sealed record FocusLostEvent : IEvent;

    /// <summary>
    /// A single key event with additional pressed modifiers.
    /// </summary>
    /// <param name="Event">The <see cref="Events.KeyEvent"/>.</param>
    public sealed record KeyEventEvent(KeyEvent Event) : IEvent;

    /// <summary>
    /// A single mouse event with additional pressed modifiers.
    /// </summary>
    /// <param name="Event">The <see cref="Events.MouseEvent"/>.</param>
    public sealed record MouseEventEvent(MouseEvent Event) : IEvent;

    /// <summary>
    /// A string that was pasted into the terminal.
    /// </summary>
    /// <param name="Text"></param>
    public sealed record TextPastedEvent(string Text) : IEvent;

    /// <summary>
    ///  An resize event with new dimensions after resize (columns, rows).
    /// </summary>
    /// <remarks>
    /// that resize events can occur in batches.
    /// </remarks>
    /// <param name="Column">The new column</param>
    /// <param name="Row">The new row</param>
    public sealed record ScreenResizeEvent(int Column, int Row) : IEvent;
}

/// <summary>
/// Represents an event.
/// </summary>
public interface IEvent
{
}
