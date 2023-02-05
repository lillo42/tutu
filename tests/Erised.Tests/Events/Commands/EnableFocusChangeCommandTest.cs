using Erised.Events;
using FluentAssertions;

namespace Erised.Tests.Events.Commands;

public class EnableFocusChangeCommandTest
{
    private readonly EnableFocusChangeCommand _command;

    public EnableFocusChangeCommandTest()
    {
        _command = new();
    }
    
    [Fact]
    public void WriteAnsi_ShouldWriteEnableFocusChangeAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}?1004h");
    }
}