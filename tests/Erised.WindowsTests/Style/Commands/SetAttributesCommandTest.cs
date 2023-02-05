using Erised.Style.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class SetAttributesCommandTest
{
    [Fact]
    public void ExecuteWindowsApi_ShouldDoNothing()
    {
        var command = new SetAttributesCommand(new List<Erised.Style.Types.Attribute>
        {
            Erised.Style.Types.Attribute.Bold,
            Erised.Style.Types.Attribute.Dim
        });

        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}