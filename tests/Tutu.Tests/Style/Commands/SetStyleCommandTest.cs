using FluentAssertions;
using Tutu.Style;
using Tutu.Style.Commands;
using Tutu.Style.Types;

namespace Tutu.Tests.Style.Commands;

public class SetStyleCommandTest
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
                    yield return [color];
                }
            }
        }
    }

    public static IEnumerable<object[]> Attributes
    {
        get
        {
            foreach (var attribute in Tutu.Style.Types.Attribute.AllAttributes)
            {
                yield return [attribute];
            }
        }
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteForegroundColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetStyleCommand(ContentStyled.Default.With(color));
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{Colored.ForegroundColor(color)}m");
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteBackgroundColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetStyleCommand(ContentStyled.Default.On(color));
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m");
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteUnderlineColorAnsiCode(Color color)
    {
        var writer = new StringWriter();
        var command = new SetStyleCommand(ContentStyled.Default.Underline(color));
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{Colored.UnderlineColor(color)}m");
    }


    [Theory]
    [MemberData(nameof(Attributes))]
    public void WriteAnsi_ShouldWriteAttribute(Tutu.Style.Types.Attribute contentAttribute)
    {
        var writer = new StringWriter();
        var command = new SetStyleCommand(ContentStyled.Default.Attribute(contentAttribute));
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}{contentAttribute.Sgr}m");
    }
}
