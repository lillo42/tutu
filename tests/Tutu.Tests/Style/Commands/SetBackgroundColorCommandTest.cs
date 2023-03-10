using FluentAssertions;
using Tutu.Style.Commands;
using Tutu.Style.Types;

namespace Tutu.Tests.Style.Commands;

public class SetBackgroundColorCommandTest
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
    public void WriteAnsi_ShouldWriteBackgroundColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetBackgroundColorCommand(color);
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m");
    }
}
