namespace Tutu.Style.Commands;

/// <summary>
/// A command that prints the given displayable type.
/// </summary>
/// <param name="Content">The content.</param>
/// <typeparam name="T">The content type.</typeparam>
public record PrintCommand<T>(T Content) : ICommand
    where T : notnull
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => write.Write(Content);

    /// <inheritdoc />
    public void ExecuteWindowsApi()
        => throw new NotSupportedException("tried to execute a print command on windows api, use ANSI instead");

    /// <inheritdoc />
    bool ICommand.IsAnsiCodeSupported => true;
}
