using AutoFixture;
using FluentAssertions;
using Tutu.Terminal.Commands;

namespace Tutu.Tests.Terminal.Commands;

public class SetTitleCommandTest
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public void WriteAnsi_ShouldWriteCorrectAnsiSequence()
    {
        var title = _fixture.Create<string>();
        var command = new SetTitleCommand(title);
        var writer = new StringWriter();
        
        command.WriteAnsi(writer);
        
        var expected = $"{AnsiCodes.ESC}]0;{title}\x07";
        writer.ToString().Should().Be(expected);
    }
}