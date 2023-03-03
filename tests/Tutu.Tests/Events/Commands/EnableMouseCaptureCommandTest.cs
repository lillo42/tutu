using FluentAssertions;
using Tutu.Events.Commands;

namespace Tutu.Tests.Events.Commands;

public class EnableMouseCaptureCommandTest
{
    private readonly EnableMouseCaptureCommand _command;

    public EnableMouseCaptureCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldWriteEnableMouseCaptureAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be(
            $"{AnsiCodes.CSI}?1000h" +
            $"{AnsiCodes.CSI}?1002h" +
            $"{AnsiCodes.CSI}?1003h" +
            $"{AnsiCodes.CSI}?1015h" +
            $"{AnsiCodes.CSI}?1006h"
        );
    }
}
