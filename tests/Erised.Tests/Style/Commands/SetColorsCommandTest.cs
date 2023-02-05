using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.Tests.Style.Commands;

public class SetColorsCommandTest
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
    public void WriteAnsi_ShouldWriteForegroundColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetColorsCommand(color, null);
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{Colored.ForegroundColor(color)}m");
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteBackgroundColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetColorsCommand(null, color);
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m");
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnis_ShouldWriteBothForegroundColorAndBackgroundColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetColorsCommand(color, color);
        command.WriteAnsi(writer);
        writer.ToString()
            .Should()
            .Be($"{AnsiCodes.CSI}{Colored.ForegroundColor(color)}m{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m");
    }
}