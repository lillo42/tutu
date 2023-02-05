using Attribute = Erised.Style.Types.Attribute;

namespace Erised.Style.Commands;

/// <summary>
/// A command that sets an attribute.
/// </summary>
/// <param name="ContentAttribute">The attribute.</param>
/// <remarks>
/// Commands must be executed/queued for execution otherwise they do nothing.
/// </remarks>
public record SetAttributeCommand(Attribute ContentAttribute) : ICommand
{
    public void WriteAnsi(TextWriter write)
        => Execute(write, ContentAttribute); 

    internal static void Execute(TextWriter write, Attribute contentAttribute)
        => write.Write($"{AnsiCodes.CSI}{contentAttribute.Sgr()}m");
    
    public void ExecuteWindowsApi() { }
}