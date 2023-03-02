using AutoFixture;
using FluentAssertions;
using Tutu.Style.Commands;

namespace Tutu.Tests.Style.Commands;

public class PrintCommandTest
{
    private readonly Fixture _fixture;

    public PrintCommandTest()
    {
        _fixture = new Fixture();
    }
    
    [Fact]
    public void WriteAnsi_ShouldWriteContent()
    {
        var content = _fixture.Create<string>();
        var writer = new StringWriter();
        new PrintCommand<string>(content).WriteAnsi(writer);
        writer.ToString().Should().Be(content);
    }
}