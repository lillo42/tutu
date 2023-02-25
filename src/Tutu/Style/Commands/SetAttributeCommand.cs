using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that sets an attribute.
/// </summary>
/// <param name="ContentAttribute">The attribute.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetAttributeCommand(Attribute ContentAttribute) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => Execute(write, ContentAttribute);

    internal static void Execute(TextWriter write, Attribute contentAttribute)
        => write.Write($"{AnsiCodes.CSI}{contentAttribute.Sgr()}m");

    /// <inheritdoc />
    public void ExecuteWindowsApi()
    {
        // Should ignore this command on windows api
    }
}