﻿using Tutu.Windows;

namespace Tutu.Cursor.Commands;

/// <summary>
/// A command that moves the terminal cursor to the given row on the current column.
/// </summary>
/// <param name="NewRow">The target row.</param>
/// <remarks>
/// <para>This command is 0 based, meaning 0 is the topmost row.</para>
/// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
/// </remarks>
public record MoveToRowCursorCommand(ushort NewRow) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write($"{AnsiCodes.CSI}{NewRow + 1}d");

    /// <inheritdoc />
    public void ExecuteWindowsApi() => WindowsCursor.MoveToRow(NewRow);
}
