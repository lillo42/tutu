using System.Runtime.InteropServices;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class EnableMouseCaptureCommandTest
{
    private readonly EnableMouseCaptureCommand _command;

    public EnableMouseCaptureCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldEnableMouseCapture()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        _command.ExecuteWindowsApi();

        var console = Windows.Console.CurrentIn;
        console.Mode.Should().Be(Windows.Event.EnableMouseMode);
    }

    [Fact]
    public void IsAnsiSupported_ShouldReturnFalse()
        => ((ICommand)_command).IsAnsiCodeSupported.Should().BeFalse();
}