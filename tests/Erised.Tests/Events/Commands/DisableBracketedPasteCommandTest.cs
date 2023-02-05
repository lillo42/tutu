using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.Tests.Events.Commands;

public class DisableBracketedPasteCommandTest
{
    private readonly DisableBracketedPasteCommand _command;

    public DisableBracketedPasteCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldWriteDisableBracketedPasteAnsiCode()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}?2004l");
    }
}