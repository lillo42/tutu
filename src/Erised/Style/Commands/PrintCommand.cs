namespace Erised.Style.Commands;

/// <summary>
/// A command that prints the given displayable type.
/// </summary>
/// <param name="Content">The content.</param>
/// <typeparam name="T"></typeparam>
public record PrintCommand<T>(T Content) : ICommand
    where T : notnull
{
    public void WriteAnsi(TextWriter write)
        => write.Write(Content);

    public void ExecuteWindowsApi() 
        => throw new NotSupportedException("tried to execute a print command on windows api, use ANSI instead");

    bool ICommand.IsAnsiCodeSupported => true;
}