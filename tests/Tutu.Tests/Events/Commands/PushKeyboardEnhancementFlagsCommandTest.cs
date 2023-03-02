using FluentAssertions;
using Tutu.Events;
using Tutu.Events.Commands;

namespace Tutu.Tests.Events.Commands;

public class PushKeyboardEnhancementFlagsCommandTest
{
    [Theory]
    [InlineData(KeyboardEnhancementFlags.DisambiguateEscapeCodes, 1)]
    [InlineData(KeyboardEnhancementFlags.ReportEventTypes, 2)]
    [InlineData(KeyboardEnhancementFlags.ReportAllKeysAsEscapeCodes, 8)]
    public void WriteAnsi_ShouldWritePushKeyboardEnhancementFlagsAnsiCode(KeyboardEnhancementFlags flags, int expectedFlag)
    {
        var command = new PushKeyboardEnhancementFlagsCommand(flags);
        var writer = new StringWriter();
        command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}>{expectedFlag}u");
    }
}