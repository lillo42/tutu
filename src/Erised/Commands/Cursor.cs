using Erised.Cursor;
using Erised.Cursor.Commands;

namespace Erised.Commands;

public static class Cursor
{
    /// <summary>
    /// A command that hides the terminal cursor.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand Hide { get; } = new HideCursorCommand();

    /// <summary>
    /// A command that shows the terminal cursor.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand Show { get; } = new ShowCursorCommand();

    /// <summary>
    /// A command that moves the terminal cursor to the given position (column, row).
    /// </summary>
    /// <param name="column">The desired column.</param>
    /// <param name="row">The desired row.</param>
    /// <remarks>
    /// * Top left cell is represented as `0,0`.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveTo(ushort column, ushort row) => new MoveToCursorCommand(column, row);

    /// <summary>
    /// A command that moves the terminal cursor down the given number of lines,
    /// and moves it to the first column.
    /// </summary>
    /// <param name="count">The number of line to move forward.</param>
    /// <remarks>
    /// * This command is 1 based, meaning `MoveToNextLine(1)` moves to the next line.
    /// * Most terminals default 0 argument to 1.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveToNextLine(ushort count) => new MoveToNextLineCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor up the given number of lines,
    /// and moves it to the first column.
    /// </summary>
    /// <param name="count"></param>
    /// <remarks>
    /// * This command is 1 based, meaning `MoveToPreviousLine(1)` moves to the previous line.
    /// * Most terminals default 0 argument to 1.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveToPreviousLine(ushort count) => new MoveToPreviousLineCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor to the given column on the current row.
    /// </summary>
    /// <param name="newColumn">The target column.</param>
    /// <remarks>
    /// * This command is 0 based, meaning 0 is the leftmost column.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveToColumn(ushort newColumn) => new MoveToColumnCursorCommand(newColumn);

    /// <summary>
    /// A command that moves the terminal cursor to the given row on the current column.
    /// </summary>
    /// <param name="newRow">The target row.</param>
    /// <remarks>
    ///* This command is 0 based, meaning 0 is the topmost row.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveRow(ushort newRow) => new MoveToRowCursorCommand(newRow);

    /// <summary>
    /// A command that moves the terminal cursor a given number of rows up.
    /// </summary>
    /// <param name="count">The number of line to be up.</param>
    /// <remarks>
    /// * This command is 1 based, meaning `MoveUp(1)` moves the cursor up one cell.
    /// * Most terminals default 0 argument to 1.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveUp(ushort count) => new MoveUpCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of columns to the right.
    /// </summary>
    /// <param name="count">The number of column to be move.</param>
    /// <remarks>
    /// * This command is 1 based, meaning `MoveRight(1)` moves the cursor right one cell.
    /// * Most terminals default 0 argument to 1.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveRight(ushort count) => new MoveRightCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of rows down.
    /// </summary>
    /// <param name="count">The number of line to be move down.</param>
    /// <remarks>
    /// * This command is 1 based, meaning `MoveDown(1)` moves the cursor down one cell.
    /// * Most terminals default 0 argument to 1.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveDown(ushort count) => new MoveDownCursorCommand(count);

    /// <summary>
    /// A command that moves the terminal cursor a given number of columns to the left.
    /// </summary>
    /// <param name="count">The number of column to be move to left.</param>
    /// <remarks>
    /// * This command is 1 based, meaning `MoveLeft(1)` moves the cursor left one cell.
    /// * Most terminals default 0 argument to 1.
    /// * Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand MoveLeft(ushort count) => new MoveLeftCursorCommand(count);

    /// <summary>
    /// A command that saves the current terminal cursor position.
    /// </summary>
    /// <remarks>
    /// - The cursor position is stored globally.
    /// - Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SavePosition { get; } = new SavePositionCursorCommand();

    /// <summary>
    /// A command that restores the saved terminal cursor position.
    /// </summary>
    /// <remarks>
    /// - The cursor position is stored globally.
    /// - Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand RestorePosition { get; } = new RestorePositionCursorCommand();


    /// <summary>
    /// A command that enables blinking of the terminal cursor.
    /// </summary>
    /// <remarks>
    /// - Some Unix terminals (ex: GNOME and Konsole) as well as Windows versions lower than Windows 10 do not support this functionality.
    ///   Use `SetCursorStyle` for better cross-compatibility.
    /// - Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand EnableBlinking { get; } = new EnableBlinkingCursorCommand();

    /// <summary>
    /// A command that disables blinking of the terminal cursor.
    /// </summary>
    /// <remarks>
    /// - Some Unix terminals (ex: GNOME and Konsole) as well as Windows versions lower than Windows 10 do not support this functionality.
    ///   Use `SetCursorStyle` for better cross-compatibility.
    /// - Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand DisableBlinking { get; } = new DisableBlinkingCursorCommand();


    /// <summary>
    /// A command that sets the style of the cursor.
    /// It uses two types of escape codes, one to control blinking, and the other the shape.
    /// </summary>
    /// <param name="style">The cursor style <see cref="CursorStyle"/>.</param>
    /// <remarks>
    /// - Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetCursorStyle(CursorStyle style) => new SetCursorStyleCursorCommand(style);

    public static (ushort Column, ushort Row) Position => Windows.Position;
}