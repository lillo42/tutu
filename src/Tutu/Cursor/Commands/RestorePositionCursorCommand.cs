using Tutu.Windows2;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that restores the saved terminal cursor position.
/// </summary>
/// <remarks>
/// <para>The cursor position is stored globally.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record RestorePositionCursorCommand : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.ESC}8");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.RestorePosition();
}