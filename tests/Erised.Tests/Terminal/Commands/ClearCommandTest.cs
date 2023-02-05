using Erised.Terminal;
using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.Tests.Terminal.Commands;

public class ClearCommandTest
{
    [Theory]
    [InlineData(ClearType.All, $"{AnsiCodes.CSI}2J")]
    [InlineData(ClearType.Purge, $"{AnsiCodes.CSI}3J")]
    [InlineData(ClearType.FromCursorDown, $"{AnsiCodes.CSI}J")]
    [InlineData(ClearType.FromCursorUp, $"{AnsiCodes.CSI}1J")]
    [InlineData(ClearType.CurrentLine, $"{AnsiCodes.CSI}2K")]
    [InlineData(ClearType.UntilNewLine, $"{AnsiCodes.CSI}K")]
    public void WriteAnsi_ShouldWriteCorrectAnsiCode(ClearType type, string expected)
    {
        var writer = new StringWriter();
        var command = new ClearCommand(type);
        command.WriteAnsi(writer);
        writer.ToString().Should().Be(expected);
    }
}