using FluentAssertions;
using Tutu.Style.Types;

namespace Tutu.Tests.Style.Types;

public class ColoredTest
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

    [Theory]
    [MemberData(nameof(Colors))]
    public void ForegroundColor_ShouldReturnNewInstance(Color color)
    {
        var foreground = Colored.ForegroundColor(color);
        foreground.Value.Should().Be(38);
        foreground.Color.Should().Be(color);
        foreground.IsForegroundColor.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void BackgroundColor_ShouldReturnNewInstance(Color color)
    {
        var background = Colored.BackgroundColor(color);
        background.Value.Should().Be(48);
        background.Color.Should().Be(color);
        background.IsBackgroundColor.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void UnderlineColor_ShouldReturnNewInstance(Color color)
    {
        var underline = Colored.UnderlineColor(color);
        underline.Value.Should().Be(58);
        underline.Color.Should().Be(color);
        underline.IsUnderlineColor.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void ParseAnsi_ShouldReturnForegroundColor(Color color)
    {
        var colored = Colored.ParseAnsi($"38;{string.Join(";", color.Values)}");
        colored.HasValue.Should().BeTrue();
        colored!.Value.Value.Should().Be(38);
        colored.Value.Color.ToString().Should().Be(color.ToString());
        colored.Value.IsForegroundColor.Should().BeTrue();


        colored = Colored.ParseAnsi($"39;{string.Join(";", color.Values)}");
        colored.HasValue.Should().BeTrue();
        colored!.Value.Value.Should().Be(38);
        colored.Value.Color.ToString().Should().Be(color.ToString());
        colored.Value.IsForegroundColor.Should().BeTrue();

    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void ParseAnsi_ShouldReturnBackgroundColor(Color color)
    {
        var colored = Colored.ParseAnsi($"48;{string.Join(";", color.Values)}");
        colored.HasValue.Should().BeTrue();
        colored!.Value.Value.Should().Be(48);
        colored.Value.Color.ToString().Should().Be(color.ToString());
        colored.Value.IsBackgroundColor.Should().BeTrue();

        colored = Colored.ParseAnsi($"49;{string.Join(";", color.Values)}");
        colored.HasValue.Should().BeTrue();
        colored!.Value.Value.Should().Be(48);
        colored.Value.Color.ToString().Should().Be(color.ToString());
        colored.Value.IsBackgroundColor.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void ParseAnsi_ShouldReturnUnderlineColor(Color color)
    {
        var colored = Colored.ParseAnsi($"58;{string.Join(";", color.Values)}");
        colored.HasValue.Should().BeTrue();
        colored!.Value.Value.Should().Be(58);
        colored.Value.Color.ToString().Should().Be(color.ToString());
        colored.Value.IsUnderlineColor.Should().BeTrue();

        colored = Colored.ParseAnsi($"59;{string.Join(";", color.Values)}");
        colored.HasValue.Should().BeTrue();
        colored!.Value.Value.Should().Be(58);
        colored.Value.Color.ToString().Should().Be(color.ToString());
        colored.Value.IsUnderlineColor.Should().BeTrue();
    }

    [Fact]
    public void ParseAnsi_ShouldReturnNull()
    {
        var colored = Colored.ParseAnsi("0");
        colored.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ParseAnsi_ShouldReturnColorReset_WhenColorIsNotSupported()
    {
        var colored = Colored.ParseAnsi("10;38;5;255");
        colored.HasValue.Should().BeTrue();
        colored!.Value.IsForegroundColor.Should().BeFalse();
        colored.Value.IsBackgroundColor.Should().BeFalse();
        colored.Value.IsUnderlineColor.Should().BeFalse();
        colored.Value.Color.Should().Be(Color.Reset);
    }
}
