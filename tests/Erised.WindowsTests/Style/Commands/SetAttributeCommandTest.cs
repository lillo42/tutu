using Erised.Style.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

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
    public void ExecuteWindowsApi_ShouldDoNoting(Erised.Style.Types.Attribute contentAttribute)
    {
        var command = new SetAttributeCommand(contentAttribute);
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}