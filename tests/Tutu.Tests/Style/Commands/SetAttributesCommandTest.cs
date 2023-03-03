using FluentAssertions;
using Tutu.Style.Commands;

namespace Tutu.Tests.Style.Commands;

public class SetAttributesCommandTest
{
    [Fact]
    public void WriteAnsi_ShouldWriteAnsi()
    {
        var command = new SetAttributesCommand(new List<Tutu.Style.Types.Attribute>
        {
            Tutu.Style.Types.Attribute.Bold,
            Tutu.Style.Types.Attribute.Dim
        });
        var writer = new StringWriter();

        command.WriteAnsi(writer);

        writer.ToString().Should().Be($"{AnsiCodes.CSI}1m{AnsiCodes.CSI}2m");
    }
}
