using FluentAssertions;
using Tutu.Style.Commands;
using Tutu.Style.Types;

namespace Tutu.Tests.Style.Commands;

public class SetForegroundColorCommandTest
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
    public void WriteAnsi_ShouldWriteAnsiCode(Color color)
    {
        var command = new SetForegroundColorCommand(color);

        var writer = new StringWriter();

        command.WriteAnsi(writer);

        var expected = $"{AnsiCodes.CSI}{Colored.ForegroundColor(color)}m";

        writer.ToString().Should().Be(expected);
    }
}