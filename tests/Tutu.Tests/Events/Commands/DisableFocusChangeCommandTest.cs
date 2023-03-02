using FluentAssertions;
using Tutu.Events.Commands;

namespace Tutu.Tests.Events.Commands;

public class DisableFocusChangeCommandTest
{
    private readonly DisableFocusChangeCommand _command;

    public DisableFocusChangeCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldWriteDisableFocusChangeAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}?1004l");
    }
}