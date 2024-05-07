using System.Runtime.InteropServices;

namespace Tutu;

/// <summary>
/// An interface for types that can directly execute commands.
/// </summary>
public interface IExecutableCommand
{
    /// <summary>
    /// Executes the given command directly.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The <see cref="IExecutableCommand"/> itself</returns>
    IExecutableCommand Execute(ICommand command);

    /// <summary>
    /// Executes the given commands directly.
    /// </summary>
    /// <param name="commands">The command.</param>
    /// <returns>The <see cref="IExecutableCommand"/> itself</returns>
    IExecutableCommand Execute(params ICommand[] commands);
}

/// <summary>
/// Default implementation of <see cref="IExecutableCommand"/>.
/// </summary>
/// <param name="Writer"></param>
public sealed record ExecutableCommand(TextWriter Writer) : IExecutableCommand
{
    /// <inheritdoc cref="IExecutableCommand.Execute(Tutu.ICommand[])"/>
    public IExecutableCommand Execute(params ICommand[] commands)
    {
        foreach (var command in commands)
        {
            Execute(command);
        }

        return this;
    }

    /// <inheritdoc cref="IExecutableCommand.Execute(Tutu.ICommand)"/>
    public IExecutableCommand Execute(ICommand command)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !command.IsAnsiCodeSupported)
        {
            command.ExecuteWindowsApi();
            return this;
        }

        command.WriteAnsi(Writer);
        Writer.Flush();
        return this;
    }
}
