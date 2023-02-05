using System.Runtime.InteropServices;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class DisableMouseCaptureCommandTest
{
    private readonly DisableMouseCaptureCommand _command;

    public DisableMouseCaptureCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldDisableMouseCapture()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        
        Windows.Event.EnableMouseCapture();

        _command.ExecuteWindowsApi();

        var console = Windows.Console.CurrentIn;
        console.Mode.Should().NotBe(Windows.Event.EnableMouseMode);
    }

    [Fact]
    public void IsAnsiSupported_ShouldReturnFalse()
        => ((ICommand)_command).IsAnsiCodeSupported.Should().BeFalse();
}