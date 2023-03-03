namespace Tutu.Events;

/// <summary>
/// The Key event. 
/// </summary>
/// <param name="Code">The <see cref="KeyCode.IKeyCode"/>.</param>
/// <param name="Modifiers">The <see cref="KeyModifiers"/>.</param>
public readonly record struct KeyEvent(KeyCode.IKeyCode Code, KeyModifiers Modifiers)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyEvent"/> struct.
    /// </summary>
    /// <param name="code">The <see cref="KeyCode.IKeyCode"/>.</param>
    /// <param name="modifiers">The <see cref="KeyModifiers"/>.</param>
    /// <param name="kind">The <see cref="KeyEventKind"/>.</param>
    public KeyEvent(KeyCode.IKeyCode code, KeyModifiers modifiers, KeyEventKind kind)
        : this(code, modifiers)
    {
        Kind = kind;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyEvent"/> struct.
    /// </summary>
    /// <param name="code">The <see cref="KeyCode.IKeyCode"/>.</param>
    /// <param name="modifiers">The <see cref="KeyModifiers"/>.</param>
    /// <param name="kind">The <see cref="KeyEventKind"/>.</param>
    /// <param name="state">The <see cref="KeyEventState"/>.</param>
    public KeyEvent(KeyCode.IKeyCode code, KeyModifiers modifiers, KeyEventKind kind, KeyEventState state)
        : this(code, modifiers, kind)
    {
        State = state;
    }

    /// <summary>
    /// The key itself.
    /// </summary>
    public KeyCode.IKeyCode Code { get; init; } = Code;

    /// <summary>
    /// Additional key modifiers.
    /// </summary>
    public KeyModifiers Modifiers { get; init; } = Modifiers;

    /// <summary>
    /// Kind of event.
    /// </summary>
    /// <remarks>
    /// Only set if <see cref="KeyboardEnhancementFlags.ReportEventTypes"/> has been enabled with
    /// <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public KeyEventKind Kind { get; init; }

    /// <summary>
    /// Keyboard state.
    /// </summary>
    /// <remarks>
    /// Only set if <see cref="KeyboardEnhancementFlags.ReportEventTypes"/> has been enabled with
    /// <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public KeyEventState State { get; init; }
}
