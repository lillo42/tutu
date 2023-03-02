using AutoFixture;
using FluentAssertions;
using Tutu.Style.Types;

namespace Tutu.Tests.Style.Types;

public class ColorTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Rgb_ShouldCreateColor()
    {
        var r = _fixture.Create<byte>();
        var g = _fixture.Create<byte>();
        var b = _fixture.Create<byte>();
        var color = Color.Rgb(r, g, b);
        color.Name.Should().StartWith("RGB");
        color.Values.Should().BeEquivalentTo(new byte[] { 2, r, g, b });
    }

    [Fact]
    public void AnsiValue_ShouldCreateColor()
    {
        var value = _fixture.Create<byte>();
        var color = Color.AnsiValue(value);
        color.Name.Should().StartWith("Ansi");
        color.Values.Should().BeEquivalentTo(new byte[] { 5, value });
    }


    public static IEnumerable<object[]> Colors
    {
        get
        {
            var colors = Color.AllColors;
            foreach (var color in colors)
            {
                if (color != Color.Reset)
                {
                    yield return new object[] { color.Name, color };
                }
            }
        }
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void TryFrom_ShouldReturnColor(string name, Color expected)
    {
        Color.TryFrom(name, out var color).Should().BeTrue();
        color.Should().Be(expected);
    }

    [Fact]
    public void TryFrom_ShouldReturnFalse()
    {
        Color.TryFrom(_fixture.Create<string>(), out _).Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(Colors))]
    public void From_ShouldReturnColor(string name, Color expected)
    {
        Color.From(name).Should().Be(expected);
    }
    
    [Fact]
    public void From_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => Color.From(_fixture.Create<string>()));
    }

   
    [Fact]
    public void ParseAnsi_ShouldReturnAnsi_WhenStartWith5()
    {
        var value = _fixture.Create<byte>();
        var color = Color.ParseAnsi($"5;{value}");
        color.HasValue.Should().BeTrue();
        color!.Value.Name.Should().StartWith("Ansi");
        color.Value.Values.Should().BeEquivalentTo(new byte[] { 5, value });
    }
    
    
    [Fact]
    public void ParseAnsi_ShouldReturnRgb_WhenStartWith2()
    {
        var r = _fixture.Create<byte>();
        var g = _fixture.Create<byte>();
        var b = _fixture.Create<byte>();
        var color = Color.ParseAnsi($"2;{r};{g};{b}");
        color.HasValue.Should().BeTrue();
        color!.Value.Name.Should().StartWith("RGB");
        color.Value.Values.Should().BeEquivalentTo(new byte[] { 2, r, g, b });
    }
    
    [Theory]
    [InlineData("5;a")]
    [InlineData("5")]
    [InlineData("2")]
    [InlineData("2;a")]
    [InlineData("2;1;b")]
    [InlineData("2;1;2;c")]
    [InlineData("20;1;2;4")]
    [InlineData("10")]
    [InlineData("ockoa")]
    public void ParseAnsi_ShouldReturnNull_WhenInvalid(string value)
    {
        var color = Color.ParseAnsi(value);
        color.HasValue.Should().BeFalse();
    }
}