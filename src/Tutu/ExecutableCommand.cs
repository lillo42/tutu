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
    /// <returns></returns>
    IExecutableCommand Execute(ICommand command);
    
    IExecutableCommand Execute(params ICommand[] commands);
}

public record ExecutableCommand(TextWriter Writer) : IExecutableCommand
{
    public IExecutableCommand Execute(params ICommand[] commands)
    {
        foreach (var command in commands)
        {
            Execute(command);
        }

        return this;
    }

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