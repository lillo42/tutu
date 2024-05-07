namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that sets the style of the cursor.
/// It uses two types of escape codes, one to control blinking, and the other the shape.
/// </summary>
/// <param name="Style">The cursor style <see cref="CursorStyle"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record SetCursorStyleCursorCommand(CursorStyle Style) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write) => write.Write(Style switch
    {
        CursorStyle.DefaultUserShape => AnsiCodes.ESC + "[0 q",
        CursorStyle.BlinkingBlock => AnsiCodes.ESC + "[1 q",
        CursorStyle.SteadyBlock => AnsiCodes.ESC + "[2 q",
        CursorStyle.BlinkingUnderScore => AnsiCodes.ESC + "[3 q",
        CursorStyle.SteadyUnderScore => AnsiCodes.ESC + "[4 q",
        CursorStyle.BlinkingBar => AnsiCodes.ESC + "[5 q",
        CursorStyle.SteadyBar => AnsiCodes.ESC + "[6 q",
        _ => throw new ArgumentOutOfRangeException()
    });

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}
