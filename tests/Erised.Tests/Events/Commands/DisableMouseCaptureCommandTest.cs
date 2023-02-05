using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.Tests.Events.Commands;

public class DisableMouseCaptureCommandTest
{
    private readonly DisableMouseCaptureCommand _command;

    public DisableMouseCaptureCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldWriteDisableMouseCaptureAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be(
            $"{AnsiCodes.CSI}?1006l" +
            $"{AnsiCodes.CSI}?1015l" +
            $"{AnsiCodes.CSI}?1003l" +
            $"{AnsiCodes.CSI}?1002l" +
            $"{AnsiCodes.CSI}?1000l"
        );
    }
}