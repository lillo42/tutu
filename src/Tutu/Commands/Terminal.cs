using Tutu.Terminal;
using Tutu.Terminal.Commands;

namespace Tutu.Commands;

/// <summary>
/// The terminal commands.
/// </summary>
public static class Terminal
{
    /// <summary>
    /// Queries the terminal's support for progressive keyboard enhancement.
    /// </summary>
    /// <param name="type">The <see cref="ClearType"/>.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand Clear(ClearType type)
        => new ClearCommand(type);

    /// <summary>
    /// Disables line wrapping.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand DisableLineWrap { get; } = new DisableLineWrapCommand();

    /// <summary>
    /// Enable line wrapping.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand EnableLineWrap { get; } = new EnableLineWrapCommand();

    /// <summary>
    /// A command that switches to alternate screen.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// Use <see cref="LeaveAlternateScreen"/>. command to leave the entered alternate screen.
    /// </remarks>
    public static ICommand EnterAlternateScreen { get; } = new EnterAlternateScreenCommand();

    /// <summary>
    /// A command that switches back to the main screen.
    /// </summary>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// Use <see cref="EnterAlternateScreen"/> to enter the alternate screen.
    /// </remarks>
    public static ICommand LeaveAlternateScreen { get; } = new LeaveAlternateScreenCommand();

    /// <summary>
    /// A command that scrolls the terminal screen a given number of rows down.
    /// </summary>
    /// <param name="lines">Number of line to be jump down.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand ScrollDown(ushort lines) => new ScrollDownCommand(lines);

    /// <summary>
    /// A command that scrolls the terminal screen a given number of rows down.
    /// </summary>
    /// <param name="lines">Number of line to be jump down.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand ScrollDown(int lines) => ScrollDown((ushort)lines);

    /// <summary>
    /// A command that scrolls the terminal screen a given number of rows up.
    /// </summary>
    /// <param name="lines">Number of line to be jump up.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand ScrollUp(ushort lines) => new ScrollUpCommand(lines);

    /// <summary>
    /// A command that scrolls the terminal screen a given number of rows up.
    /// </summary>
    /// <param name="lines">Number of line to be jump up.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand ScrollUp(int lines) => ScrollUp((ushort)lines);

    /// <summary>
    /// A command that sets the terminal buffer size `(columns, rows)`.
    /// </summary>
    /// <param name="column">The new column size.</param>
    /// <param name="row">The new row size.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetSize(ushort column, ushort row) => new SetSizeCommand(column, row);

    /// <summary>
    /// A command that sets the terminal buffer size `(columns, rows)`.
    /// </summary>
    /// <param name="column">The new column size.</param>
    /// <param name="row">The new row size.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetSize(int column, int row) => SetSize((ushort)column, (ushort)row);

    /// <summary>
    /// A command that sets the terminal title
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <remarks>
    /// Commands must be executed/queued for execution otherwise they do nothing.
    /// </remarks>
    public static ICommand SetTitle(string title) => new SetTitleCommand(title);
}