using AutoFixture;
using FluentAssertions;
using Tutu.Style;
using Tutu.Style.Commands;
using Tutu.Style.Types;

namespace Tutu.Tests.Style.Commands;

public class PrintStyledContentCommandTest
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

    private readonly Fixture _fixture = new();

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteWithBackground(Color color)
    {
        var text = _fixture.Create<string>();

        var writer = new StringWriter();

        var command = new PrintStyledContentCommand<string>(new StyledContent<string>(
            ContentStyled.Default with { BackgroundColor = color },
            text
        ));

        command.WriteAnsi(writer);

        var expected =
            $"{AnsiCodes.CSI}{Colored.BackgroundColor(color)}m{text}{AnsiCodes.CSI}{Colored.BackgroundColor(Color.Reset)}m";
        writer.ToString().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteWithForeground(Color color)
    {
        var text = _fixture.Create<string>();

        var writer = new StringWriter();

        var command = new PrintStyledContentCommand<string>(new StyledContent<string>(
            ContentStyled.Default with { ForegroundColor = color },
            text
        ));

        command.WriteAnsi(writer);

        var expected =
            $"{AnsiCodes.CSI}{Colored.ForegroundColor(color)}m{text}{AnsiCodes.CSI}{Colored.ForegroundColor(Color.Reset)}m";
        writer.ToString().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteWithUnderline(Color color)
    {
        var text = _fixture.Create<string>();

        var writer = new StringWriter();

        var command = new PrintStyledContentCommand<string>(new StyledContent<string>(
            ContentStyled.Default with { UnderlineColor = color },
            text
        ));

        command.WriteAnsi(writer);

        var expected =
            $"{AnsiCodes.CSI}{Colored.UnderlineColor(color)}m{text}{AnsiCodes.CSI}{Colored.ForegroundColor(Color.Reset)}m";
        writer.ToString().Should().Be(expected);
    }


    [Theory]
    [MemberData(nameof(Attributes))]
    public void WriteAnsi_ShouldWriteWithAttribute(Tutu.Style.Types.Attribute attribute)
    {
        var text = _fixture.Create<string>();

        var writer = new StringWriter();

        var command = new PrintStyledContentCommand<string>(new StyledContent<string>(
            ContentStyled.Default.Attribute(attribute),
            text
        ));

        command.WriteAnsi(writer);

        var expected = $"{AnsiCodes.CSI}{attribute.Sgr}m{text}{AnsiCodes.CSI}{Tutu.Style.Types.Attribute.Reset.Sgr}m";
        writer.ToString().Should().Be(expected);
    }
}
