using AutoFixture;
using FluentAssertions;
using Tutu.Terminal.Commands;

namespace Tutu.Tests.Terminal.Commands;

public class SetSizeCommandTest
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void WriteAnsi_ShouldWriteCorrectAnsiSequence()
    {
        var width = _fixture.Create<ushort>();
        var height = _fixture.Create<ushort>();
        var command = new SetSizeCommand(width, height);
        var writer = new StringWriter();

        command.WriteAnsi(writer);

        var expected = $"{AnsiCodes.CSI}8;{width};{height}t";
        writer.ToString().Should().Be(expected);
    }
}
