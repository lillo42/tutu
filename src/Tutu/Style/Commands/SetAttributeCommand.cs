using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that sets an attribute.
/// </summary>
/// <param name="Attribute">The attribute.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public sealed record SetAttributeCommand(Attribute Attribute) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => Execute(write, Attribute);

    internal static void Execute(TextWriter write, Attribute contentAttribute)
        => write.Write($"{AnsiCodes.CSI}{contentAttribute.Sgr}m");

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}
