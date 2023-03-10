namespace Tutu.Events;

/// <summary>
/// Represents a mouse event.
/// 
/// Some platforms/terminals do not report mouse button for the
/// <see cref="MouseEventKind.MouseUpEventKind"/> and <see cref="MouseEventKind.MouseDragEventKind"/> events. <see cref="MouseButton.Left"/>
/// is returned if we don't know which button was used.
///
/// Some platforms/terminals does not report all key modifiers
/// combinations for all mouse event types. For example - macOS reports
/// `Ctrl` + left mouse button click as a right mouse button click.
/// </summary>
public readonly record struct MouseEvent(MouseEventKind.IMouseEventKind Kind, int Column, int Row, KeyModifiers Modifiers)
{
    /// <summary>
    /// The kind of mouse event that was caused.
    /// </summary>
    public MouseEventKind.IMouseEventKind Kind { get; init; } = Kind;

    /// <summary>
    /// The column that the event occurred on.
    /// </summary>
    public int Column { get; init; } = Column;

    /// <summary>
    /// The row that the event occurred on.
    /// </summary>
    public int Row { get; init; } = Row;

    /// <summary>
    /// The key modifiers active when the event occurred.
    /// </summary>
    public KeyModifiers Modifiers { get; init; } = Modifiers;
}
