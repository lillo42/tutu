using System.Runtime.InteropServices;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class PopKeyboardEnhancementFlagsCommandTest
{
    private readonly PopKeyboardEnhancementFlagsCommand _command;

    public PopKeyboardEnhancementFlagsCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldDoThrowNotSupported()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        _command
            .Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .Throw<NotSupportedException>();
    }

    [Fact]
    public void IsApplicable_ShouldReturnFalse()
        => ((ICommand)_command).IsAnsiCodeSupported.Should().BeFalse();
}