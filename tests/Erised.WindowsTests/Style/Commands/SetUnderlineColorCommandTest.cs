using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class SetUnderlineColorCommandTest
{
    public static IEnumerable<object[]> Colors
    {
        get
        {
            var colors = Color.AllColors;
            foreach (var color in colors)
            {
                if (color != Color.Reset)
                {
                    yield return new object[] { color };
                }
            }
        }
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void ExecuteWindowsApi_ShouldThrowNotSupported(Color color)
    {
        var command = new SetUnderlineColorCommand(color);
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .Throw<NotSupportedException>();
    }
}