using System.Runtime.InteropServices;
using Erised.Events;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class PushKeyboardEnhancementFlagsCommandTest
{
    [Theory]
    [InlineData(KeyboardEnhancementFlags.DisambiguateEscapeCodes)]
    [InlineData(KeyboardEnhancementFlags.ReportEventTypes)]
    [InlineData(KeyboardEnhancementFlags.ReportAllKeysAsEscapeCodes)]
    public void ExecuteWindowsApi_ShouldDoThrowNotSupported(KeyboardEnhancementFlags flags)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        new PushKeyboardEnhancementFlagsCommand(flags)
            .Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .Throw<NotSupportedException>();
    }

    [Theory]
    [InlineData(KeyboardEnhancementFlags.DisambiguateEscapeCodes)]
    [InlineData(KeyboardEnhancementFlags.ReportEventTypes)]
    [InlineData(KeyboardEnhancementFlags.ReportAllKeysAsEscapeCodes)]
    public void IsApplicable_ShouldReturnFalse(KeyboardEnhancementFlags flags)
        => ((ICommand)new PushKeyboardEnhancementFlagsCommand(flags))
            .IsAnsiCodeSupported.Should().BeFalse();
}