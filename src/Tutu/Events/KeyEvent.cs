namespace Tutu.Events;

/// <summary>
/// 
/// </summary>
public readonly record struct KeyEvent(KeyCode.IKeyCode Code, KeyModifiers Modifiers)
{
    public KeyEvent(KeyCode.IKeyCode code, KeyModifiers modifiers, KeyEventKind kind)
        : this(code, modifiers)
    {
        Kind = kind;
    }

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
    ///
    /// Only set if <see cref="KeyboardEnhancementFlags.ReportEventTypes"/> has been enabled with [`PushKeyboardEnhancementFlags`].
    /// </summary>
    public KeyEventKind Kind { get; init; }

    /// <summary>
    /// Keyboard state.
    ///
    /// Only set if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/> has been enabled with
    /// [`PushKeyboardEnhancementFlags`].
    /// </summary>
    public KeyEventState State { get; init; }


    public KeyEvent NormalizeCase()
    {
        if (Code is not KeyCode.CharKeyCode keyCode)
        {
            return this;
        }

        if (char.IsUpper(keyCode.Character))
        {
            return this with { Modifiers = Modifiers | KeyModifiers.Shift };
        }

        if (!Modifiers.HasFlag(KeyModifiers.Shift))
        {
            return this with { Code = new KeyCode.CharKeyCode(char.ToUpper(keyCode.Character))};
        }

        return this;
    }
}