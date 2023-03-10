using AutoFixture;
using FluentAssertions;
using Tutu.Terminal.Commands;

namespace Tutu.Tests.Terminal.Commands;

public class ScrollUpCommandTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void WriteAnsi_ShouldWriteCorrectAnsiSequence()
    {
        var lines = _fixture.Create<ushort>();
        var command = new ScrollUpCommand(lines);
        var writer = new StringWriter();

        command.WriteAnsi(writer);

        var expected = $"{AnsiCodes.CSI}{lines}S";
        writer.ToString().Should().Be(expected);
    }
}
