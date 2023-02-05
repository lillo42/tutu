namespace Erised.Cursor;

/// <summary>
/// A command that sets the style of the cursor.
/// It uses two types of escape codes, one to control blinking, and the other the shape.
/// </summary>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public enum CursorStyle
{
    /// <summary>
    /// Default cursor shape configured by the user.
    /// </summary>
    DefaultUserShape,

    /// <summary>
    /// A blinking block cursor shape (■).
    /// </summary>
    BlinkingBlock,

    /// <summary>
    /// A non blinking block cursor shape (inverse of `BlinkingBlock`).
    /// </summary>
    SteadyBlock,

    /// <summary>
    /// A blinking underscore cursor shape(_).
    /// </summary>
    BlinkingUnderScore,

    /// <summary>
    /// A non blinking underscore cursor shape (inverse of `BlinkingUnderScore`).
    /// </summary>
    SteadyUnderScore,


    /// <summary>
    /// A blinking cursor bar shape (|)
    /// </summary>
    BlinkingBar,

    /// <summary>
    /// A steady cursor bar shape (inverse of `BlinkingBar`).
    /// </summary>
    SteadyBar,
}