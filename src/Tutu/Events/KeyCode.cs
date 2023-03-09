namespace Tutu.Events;

/// <summary>
/// Represents a key.
/// </summary>
public static class KeyCode
{
    /// <summary>
    /// Backspace key.
    /// </summary>
    public static IKeyCode Backspace { get; } = new BackspaceKeyCode();

    /// <summary>
    /// Enter key.
    /// </summary>
    public static IKeyCode Enter { get; } = new EnterKeyCode();

    /// <summary>
    /// Left arrow key.
    /// </summary>
    public static IKeyCode Left { get; } = new LeftKeyCode();

    /// <summary>
    /// Right arrow key.
    /// </summary>
    public static IKeyCode Right { get; } = new RightKeyCode();

    /// <summary>
    /// Up arrow key.
    /// </summary>
    public static IKeyCode Up { get; } = new UpKeyCode();

    /// <summary>
    /// Down arrow key.
    /// </summary>
    public static IKeyCode Down { get; } = new DownKeyCode();

    /// <summary>
    /// Home key.
    /// </summary>
    public static IKeyCode Home { get; } = new HomeKeyCode();

    /// <summary>
    /// End key.
    /// </summary>
    public static IKeyCode End { get; } = new EndKeyCode();

    /// <summary>
    /// Page up key.
    /// </summary>
    public static IKeyCode PageUp { get; } = new PageUpKeyCode();

    /// <summary>
    /// Page down key.
    /// </summary>
    public static IKeyCode PageDown { get; } = new PageDownKeyCode();

    /// <summary>
    /// Tab key.
    /// </summary>
    public static IKeyCode Tab { get; } = new TabKeyCode();

    /// <summary>
    /// Back tab key.
    /// </summary>
    public static IKeyCode BackTab { get; } = new BackTabKeyCode();

    /// <summary>
    /// Delete key.
    /// </summary>
    public static IKeyCode Delete { get; } = new DeleteKeyCode();

    /// <summary>
    /// Insert key.
    /// </summary>
    public static IKeyCode Insert { get; } = new InsertKeyCode();

    /// <summary>
    /// F key.
    ///
    /// `KeyCode::F(1)` represents F1 key, etc.
    /// </summary>
    /// <returns>New instance <see cref="FKeyCode"/>.</returns>
    public static IKeyCode F(int number) => new FKeyCode(number);

    /// <summary>
    /// A character.
    /// </summary>
    /// <param name="c">The pressed char</param>
    /// <returns>New instance <see cref="CharKeyCode"/>.</returns>
    public static IKeyCode Char(char c) => Char(c.ToString());

    /// <summary>
    /// A character.
    /// </summary>
    /// <param name="c">The pressed char</param>
    /// <returns>New instance <see cref="CharKeyCode"/>.</returns>
    public static IKeyCode Char(string c) => new CharKeyCode(c);

    /// <summary>
    /// Null key.
    /// </summary>
    public static IKeyCode Null { get; } = new NullKeyCode();

    /// <summary>
    /// Esc key.
    /// </summary>
    public static IKeyCode Esc { get; } = new EscKeyCode();

    /// <summary>
    /// Caps lock key.
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Events.Event"/>
    /// </remarks>
    public static IKeyCode CapsLock { get; } = new CapsLockKeyCode();

    /// <summary>
    /// Scroll lock key.
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode ScrollLock { get; } = new ScrollLockKeyCode();

    /// <summary>
    /// Num Lock key.
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks> 
    public static IKeyCode NumLock { get; } = new NumLockKeyCode();

    /// <summary>
    /// Print Screen key.
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode PrintScreen { get; } = new PrintScreenKeyCode();

    /// <summary>
    /// Pause key. 
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode Pause { get; } = new PauseKeyCode();

    /// <summary>
    /// Menu key.
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode Menu { get; } = new MenuKeyCode();

    /// <summary>
    /// The "Begin" key (often mapped to the 5 key when Num Lock is turned on).
    /// </summary>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode KeypadBegin { get; } = new KeypadBeginKeyCode();

    /// <summary>
    /// A modifier key. 
    /// </summary>
    /// <param name="code">The <see cref="Events.MediaKeyCode"/> pressed.</param>
    /// <returns>New instance of <see cref="KeyCode.MediaKeyCode"/>.</returns>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode Media(Events.MediaKeyCode code) => new MediaKeyCode(code);

    /// <summary>
    /// A modifier key. 
    /// </summary>
    /// <param name="code"></param>
    /// <returns>New instance of <see cref="ModifierKeyCode"/>.</returns>
    /// <remarks>
    /// this key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>
    /// </remarks>
    public static IKeyCode Modifier(Events.ModifierKeyCode code) => new ModifierKeyCode(code);

    /// <summary>
    /// Represents a key.
    /// </summary>
    public interface IKeyCode
    {
    }

    /// <summary>
    /// Backspace key.
    /// </summary>
    public record BackspaceKeyCode : IKeyCode;

    /// <summary>
    /// Enter key.
    /// </summary>
    public record EnterKeyCode : IKeyCode;

    /// <summary>
    /// Left arrow key.
    /// </summary>
    public record LeftKeyCode : IKeyCode;

    /// <summary>
    /// Right arrow key.
    /// </summary>
    public record RightKeyCode : IKeyCode;

    /// <summary>
    /// Up arrow key.
    /// </summary>
    public record UpKeyCode : IKeyCode;

    /// <summary>
    /// Down arrow key.
    /// </summary>
    public record DownKeyCode : IKeyCode;

    /// <summary>
    /// Home key.
    /// </summary>
    public record HomeKeyCode : IKeyCode;

    /// <summary>
    /// End key.
    /// </summary>
    public record EndKeyCode : IKeyCode;

    /// <summary>
    /// Page up key.
    /// </summary>
    public record PageUpKeyCode : IKeyCode;

    /// <summary>
    /// Page down key.
    /// </summary>
    public record PageDownKeyCode : IKeyCode;

    /// <summary>
    /// Tab key.
    /// </summary>
    public record TabKeyCode : IKeyCode;

    /// <summary>
    /// Shift + Tab key.
    /// </summary>
    public record BackTabKeyCode : IKeyCode;

    /// <summary>
    /// Delete key.
    /// </summary>
    public record DeleteKeyCode : IKeyCode;

    /// <summary>
    /// Insert key.
    /// </summary>
    public record InsertKeyCode : IKeyCode;

    /// <summary>
    /// F key.
    ///
    /// `KeyCode::F(1)` represents F1 key, etc.
    /// </summary>
    public record FKeyCode(int Number) : IKeyCode
    {
        /// <inheritdoc />
        public override string ToString() => $"F{Number}";
    }

    /// <summary>
    /// A character.
    /// </summary>
    /// <param name="Character">The character.</param>
    /// <remarks>
    /// Because we need to support UTF-8 character, not all value can be inside in <see langword="char"/>. 
    /// </remarks>
    public record CharKeyCode(string Character) : IKeyCode;

    /// <summary>
    /// Null.
    /// </summary>
    public record NullKeyCode : IKeyCode;

    /// <summary>
    /// Escape key.
    /// </summary>
    public record EscKeyCode : IKeyCode;

    /// <summary>
    /// Caps Lock key.
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record CapsLockKeyCode : IKeyCode;

    /// <summary>
    /// Scroll Lock key.
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record ScrollLockKeyCode : IKeyCode;

    /// <summary>
    /// Num Lock key.
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record NumLockKeyCode : IKeyCode;

    /// <summary>
    /// Print Screen key.
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record PrintScreenKeyCode : IKeyCode;

    /// <summary>
    /// Pause key.
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record PauseKeyCode : IKeyCode;

    /// <summary>
    /// Menu key.
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record MenuKeyCode : IKeyCode;

    /// <summary>
    /// The "Begin" key (often mapped to the 5 key when Num Lock is turned on).
    /// </summary>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record KeypadBeginKeyCode : IKeyCode;

    /// <summary>
    /// A media key.
    /// </summary>
    /// <param name="Media">The <see cref="Events.MediaKeyCode"/>.</param>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record MediaKeyCode(Events.MediaKeyCode Media) : IKeyCode;

    /// <summary>
    /// A modifier key.
    /// </summary>
    /// <param name="Modifier">The <see cref="Events.ModifierKeyCode"/>.</param>
    /// <remarks>
    /// This key can only be read if <see cref="KeyboardEnhancementFlags.DisambiguateEscapeCodes"/>
    /// has been enabled with <see cref="Tutu.Commands.Events.PushKeyboardEnhancementFlags"/>.
    /// </remarks>
    public record ModifierKeyCode(Events.ModifierKeyCode Modifier) : IKeyCode;
}
