using FluentAssertions;
using Tutu.Style.Commands;

namespace Tutu.Tests.Style.Commands;

public class ResetColorCommandTest
{
    private readonly ResetColorCommand _command;

    public ResetColorCommandTest()
    {
        _command = new();
    }
    
    [Fact]
    public void WriteAnsi_ShouldWriteResetColorAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}0m");
    }
}