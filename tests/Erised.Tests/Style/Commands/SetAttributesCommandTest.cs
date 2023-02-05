using Erised.Style.Commands;
using FluentAssertions;

namespace Erised.Tests.Style.Commands;

public class SetAttributesCommandTest
{
    [Fact]
    public void WriteAnsi_ShouldWriteAnsi()
    {
        var command = new SetAttributesCommand(new List<Erised.Style.Types.Attribute>
        {
            Erised.Style.Types.Attribute.Bold,
            Erised.Style.Types.Attribute.Dim
        });
        var writer = new StringWriter();

        command.WriteAnsi(writer);

        writer.ToString().Should().Be($"{AnsiCodes.CSI}1m{AnsiCodes.CSI}2m");
    }
}