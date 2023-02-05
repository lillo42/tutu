namespace Erised.Events;

/// <summary>
/// A mouse event kind.
///
/// Some platforms/terminals do not report mouse button for the
/// <see cref="MouseUpEventKind"/> and <see cref="MouseDragEventKind"/> events. <see cref="MouseButton.Left"/>
/// is returned if we don't know which button was used.
/// </summary>
public static class MouseEventKind
{
    /// <summary>
    /// pressed mouse button. contains the button that was pressed.
    /// </summary>
    /// <param name="button">The pressed <see cref="MouseButton"/>.</param>
    public static IMouseEventKind Down(MouseButton button) => new MouseDownEventKind(button);

    /// <summary>
    /// Released mouse button. Contains the button that was released.
    /// </summary>
    /// <param name="button">The pressed <see cref="MouseButton"/>.</param>
    /// <returns>New instance of <see cref="MouseUpEventKind"/>.</returns>
    public static IMouseEventKind Up(MouseButton button) => new MouseUpEventKind(button);

    /// <summary>
    /// Moved the mouse cursor while pressing the contained mouse button.
    /// </summary>
    /// <param name="button">The pressed <see cref="MouseButton"/>.</param>
    /// <remarks>New instace of <see cref="MouseDragEventKind"/>.</remarks>
    public static IMouseEventKind Drag(MouseButton button) => new MouseDragEventKind(button);

    /// <summary>
    /// Moved the mouse cursor while not pressing a mouse button.
    /// </summary>
    public static IMouseEventKind Moved { get; } = new MouseMovedEventKind();

    /// <summary>
    /// Scrolled mouse wheel downwards (towards the user).
    /// </summary>
    public static IMouseEventKind ScrollDown { get; } = new MouseScrollDownEventKind();

    /// <summary>
    /// Scrolled mouse wheel upwards (away from the user).
    /// </summary>
    public static IMouseEventKind ScrollUp { get; } = new MouseScrollUpEventKind();

    /// <summary>
    /// A mouse event kind.
    ///
    /// Some platforms/terminals do not report mouse button for the
    /// <see cref="MouseUpEventKind"/> and <see cref="MouseDragEventKind"/> events. <see cref="MouseButton.Left"/>
    /// is returned if we don't know which button was used.
    /// </summary>
    public interface IMouseEventKind
    {
    }

    /// <summary>
    /// pressed mouse button. contains the button that was pressed.
    /// </summary>
    /// <param name="Button">The pressed <see cref="MouseButton"/>.</param>
    public record MouseDownEventKind(MouseButton Button) : IMouseEventKind;

    /// <summary>
    /// Released mouse button. Contains the button that was released.
    /// </summary>
    /// <param name="Button">The pressed <see cref="MouseButton"/>.</param>
    public record MouseUpEventKind(MouseButton Button) : IMouseEventKind;

    /// <summary>
    /// Moved the mouse cursor while pressing the contained mouse button.
    /// </summary>
    /// <param name="Button">The pressed <see cref="MouseButton"/>.</param>
    public record MouseDragEventKind(MouseButton Button) : IMouseEventKind;

    /// <summary>
    /// Moved the mouse cursor while not pressing a mouse button.
    /// </summary>
    public record MouseMovedEventKind : IMouseEventKind;

    /// <summary>
    /// Scrolled mouse wheel downwards (towards the user).
    /// </summary>
    public record MouseScrollDownEventKind : IMouseEventKind;

    /// <summary>
    /// Scrolled mouse wheel upwards (away from the user).
    /// </summary>
    public record MouseScrollUpEventKind : IMouseEventKind;
}