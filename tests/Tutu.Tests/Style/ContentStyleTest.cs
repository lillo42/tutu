using FluentAssertions;
using Tutu.Style;
using Tutu.Style.Types;
using Attribute = Tutu.Style.Types.Attribute;

namespace Tutu.Tests.Style;

public class ContentStyleTest
{
    [Fact]
    public void Set()
    {
        var style = ContentStyled.Default
            .With(Color.Blue)
            .On(Color.Red)
            .Attribute(Attribute.Bold);

        style.ForegroundColor.Should().Be(Color.Blue);
        style.BackgroundColor.Should().Be(Color.Red);
        style.Attributes.Should().Contain(Attribute.Bold);
    }


    [Fact]
    public void Apply()
    {
        const string content = "test";
        var styledContent = ContentStyled.Default
            .With(Color.Blue)
            .On(Color.Red)
            .Attribute(Attribute.Bold)
            .Apply(content);
        
        styledContent.Style.ForegroundColor.Should().Be(Color.Blue);
        styledContent.Style.BackgroundColor.Should().Be(Color.Red);
        styledContent.Style.Attributes.Should().Contain(Attribute.Bold);
        styledContent.Content.Should().Be(content);
    }
}