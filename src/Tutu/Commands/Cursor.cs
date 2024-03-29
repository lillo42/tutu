﻿using Tutu.Cursor;
using Tutu.Cursor.Commands;

namespace Tutu.Commands;

/// <summary>
/// The cursor commands.
/// </summary>
public static class Cursor
{
    /// <inheritdoc cref="HideCursorCommand"/>
    public static ICommand Hide { get; } = new HideCursorCommand();

    /// <inheritdoc cref="ShowCursorCommand"/>
    public static ICommand Show { get; } = new ShowCursorCommand();

    /// <inheritdoc cref="MoveToCursorCommand"/>
    public static ICommand MoveTo(ushort column, ushort row) => new MoveToCursorCommand(column, row);

    /// <summary>
    /// A command that moves the terminal cursor to the given position (column, row).
    /// </summary>
    /// <param name="column">The desired column.</param>
    /// <param name="row">The desired row.</param>
    /// <remarks>
    /// <para>Top left cell is represented as `0,0`.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveTo(int column, int row) => MoveTo((ushort)column, (ushort)row);

    /// <summary>
    /// A command that moves the terminal cursor down the given number of lines,
    /// and moves it to the first column.
    /// </summary>
    /// <param name="count">The number of line to move forward.</param>
    /// <remarks>
    /// <para> This command is 1 based, meaning `MoveToNextLine(1)` moves to the next line.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveToNextLine(ushort count) => new MoveToNextLineCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor down the given number of lines,
    /// and moves it to the first column.
    /// </summary>
    /// <param name="count">The number of line to move forward.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveToNextLine(1)` moves to the next line.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveToNextLine(int count) => MoveToNextLine((ushort)count);

    /// <summary>
    /// A command that moves the terminal cursor up the given number of lines,
    /// and moves it to the first column.
    /// </summary>
    /// <param name="count"></param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveToPreviousLine(1)` moves to the previous line.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveToPreviousLine(ushort count) => new MoveToPreviousLineCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor up the given number of lines,
    /// and moves it to the first column.
    /// </summary>
    /// <param name="count"></param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveToPreviousLine(1)` moves to the previous line.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveToPreviousLine(int count) => MoveToPreviousLine((ushort)count);

    /// <summary>
    /// A command that moves the terminal cursor to the given column on the current row.
    /// </summary>
    /// <param name="newColumn">The target column.</param>
    /// <remarks>
    /// <para>This command is 0 based, meaning 0 is the leftmost column.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveToColumn(ushort newColumn) => new MoveToColumnCursorCommand(newColumn);

    /// <summary>
    /// A command that moves the terminal cursor to the given column on the current row.
    /// </summary>
    /// <param name="newColumn">The target column.</param>
    /// <remarks>
    /// <para>This command is 0 based, meaning 0 is the leftmost column.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveToColumn(int newColumn) => MoveToColumn((ushort)newColumn);

    /// <summary>
    /// A command that moves the terminal cursor to the given row on the current column.
    /// </summary>
    /// <param name="newRow">The target row.</param>
    /// <remarks>
    /// <para>This command is 0 based, meaning 0 is the topmost row.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveRow(ushort newRow) => new MoveToRowCursorCommand(newRow);

    /// <summary>
    /// A command that moves the terminal cursor to the given row on the current column.
    /// </summary>
    /// <param name="newRow">The target row.</param>
    /// <remarks>
    /// <para>This command is 0 based, meaning 0 is the topmost row.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveRow(int newRow) => MoveRow((ushort)newRow);

    /// <summary>
    /// A command that moves the terminal cursor a given number of rows up.
    /// </summary>
    /// <param name="count">The number of line to be up.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveUp(1)` moves the cursor up one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveUp(ushort count) => new MoveUpCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of rows up.
    /// </summary>
    /// <param name="count">The number of line to be up.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveUp(1)` moves the cursor up one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveUp(int count) => MoveUp((ushort)count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of columns to the right.
    /// </summary>
    /// <param name="count">The number of column to be move.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveRight(1)` moves the cursor right one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveRight(ushort count) => new MoveRightCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of columns to the right.
    /// </summary>
    /// <param name="count">The number of column to be move.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveRight(1)` moves the cursor right one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveRight(int count) => MoveRight((ushort)count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of rows down.
    /// </summary>
    /// <param name="count">The number of line to be move down.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveDown(1)` moves the cursor down one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveDown(ushort count) => new MoveDownCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of rows down.
    /// </summary>
    /// <param name="count">The number of line to be move down.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveDown(1)` moves the cursor down one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveDown(int count) => MoveDown((ushort)count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of columns to the left.
    /// </summary>
    /// <param name="count">The number of column to be move to left.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveLeft(1)` moves the cursor left one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveLeft(ushort count) => new MoveLeftCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of columns to the left.
    /// </summary>
    /// <param name="count">The number of column to be move to left.</param>
    /// <remarks>
    /// <para>This command is 1 based, meaning `MoveLeft(1)` moves the cursor left one cell.</para>
    /// <para>Most terminals default 0 argument to 1.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand MoveLeft(int count) => MoveLeft((ushort)count);

    /// <summary>
    /// A command that saves the current terminal cursor position.
    /// </summary>
    /// <remarks>
    /// <para>The cursor position is stored globally.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand SavePosition { get; } = new SavePositionCursorCommand();

    /// <summary>
    /// A command that restores the saved terminal cursor position.
    /// </summary>
    /// <remarks>
    /// <para>The cursor position is stored globally.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand RestorePosition { get; } = new RestorePositionCursorCommand();

    /// <summary>
    /// A command that enables blinking of the terminal cursor.
    /// </summary>
    /// <remarks>
    /// <para> Some Unix terminals (ex: GNOME and Konsole) as well as Windows versions lower than Windows 10 do not support this functionality.</para>
    /// <para>Use <see cref="SetCursorStyle"/> for better cross-compatibility.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand EnableBlinking { get; } = new EnableBlinkingCursorCommand();

    /// <summary>
    /// A command that disables blinking of the terminal cursor.
    /// </summary>
    /// <remarks>
    /// <para>Some Unix terminals (ex: GNOME and Konsole) as well as Windows versions lower than Windows 10 do not support this functionality.</para>
    /// <para>Use <see cref="SetCursorStyle"/> for better cross-compatibility.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand DisableBlinking { get; } = new DisableBlinkingCursorCommand();

    /// <summary>
    /// A command that sets the style of the cursor.
    /// </summary>
    /// <param name="style">The cursor style <see cref="CursorStyle"/>.</param>
    /// <remarks>
    /// <para>It uses two types of escape codes, one to control blinking, and the other the shape.</para>
    /// <para>Commands must be executed/queued for execution otherwise they do nothing.</para>
    /// </remarks>
    public static ICommand SetCursorStyle(CursorStyle style) => new SetCursorStyleCursorCommand(style);
}
