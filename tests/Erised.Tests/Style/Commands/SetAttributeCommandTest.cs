using Erised.Style.Commands;
using FluentAssertions;

namespace Erised.Tests.Style.Commands;

public class SetAttributeCommandTest
{
    public static IEnumerable<object[]> Attributes
    {
        get
        {
            foreach (var attribute in Erised.Style.Types.Attribute.AllAttributes)
            {
                yield return new object[] { attribute };
            }
        }
    }

    [Theory]
    [MemberData(nameof(Attributes))]
    public void WriteAnsi_ShouldWriteAttribute(Erised.Style.Types.Attribute contentAttribute)
    {
        var writer = new StringWriter();

        var command = new SetAttributeCommand(contentAttribute);
        command.WriteAnsi(writer);

        writer.ToString()
            .Should()
            .Be($"{AnsiCodes.CSI}{contentAttribute.Sgr()}m");
    }
}