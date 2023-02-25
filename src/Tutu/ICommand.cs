namespace Tutu;

/// <summary>
/// An interface for a command that performs an action on the terminal.
/// </summary>
/// <remarks>
/// Tutu provides a set of commands,
/// and there is no immediate reason to implement a command yourself.
/// In order to understand how to use and execute commands.
/// </remarks>
public interface ICommand
{
    /// <summary>
    /// Write an ANSI representation of this command to the given writer.
    /// An ANSI code can manipulate the terminal by writing it to the terminal buffer.
    /// However, only Windows 10 and UNIX systems support this.
    /// </summary>
    /// <param name="write"></param>
    /// <remarks>
    /// This method does not need to be accessed manually, as it is used by the Tutu. 
    /// </remarks>
    void WriteAnsi(TextWriter write);

    /// <summary>
    /// Returns whether the ANSI code representation of this command is supported by windows.
    /// </summary>
    /// <remarks>
    /// A list of supported ANSI escape codes
    /// can be found <see href="https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences">here</see>
    /// </remarks>
    bool IsAnsiCodeSupported => AnsiSupport.IsAnsiSupported;

    /// <summary>
    /// Execute this command using Windows API.
    /// </summary>
    /// <remarks>
    /// Windows versions lower than windows 10 do not support ANSI escape codes,
    /// therefore a direct WinAPI call is made.
    /// </remarks>
    void ExecuteWindowsApi();
}