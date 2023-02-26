using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Style.Commands;

/// <summary>
/// A command that sets several attributes.
/// </summary>
/// <param name="Attributes">A collection of <see cref="Attribute"/>.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetAttributesCommand(IEnumerable<Attribute> Attributes) : ICommand
{
    /// <inheritdoc />
    public void WriteAnsi(TextWriter write)
        => Execute(write, Attributes);

    internal static void Execute(TextWriter write, IEnumerable<Attribute> contentAttributes)
    {
        foreach (var attribute in contentAttributes)
        {
            SetAttributeCommand.Execute(write, attribute);
        }
    }

    /// <inheritdoc />
    public void ExecuteWindowsApi() { }
}