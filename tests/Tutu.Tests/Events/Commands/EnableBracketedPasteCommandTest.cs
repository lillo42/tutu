using FluentAssertions;
using Tutu.Events.Commands;

namespace Tutu.Tests.Events.Commands;

public class EnableBracketedPasteCommandTest
{
    private readonly EnableBracketedPasteCommand _command;

    public EnableBracketedPasteCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldWriteEnableBracketedPasteAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}?2004h");
    }
}
