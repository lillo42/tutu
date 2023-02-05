using System.Runtime.InteropServices;
using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class DisableLineWrapCommandTest
{
    private readonly DisableLineWrapCommand _command;

    public DisableLineWrapCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldDisableLine()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        var mode = new Windows.Console(Windows.ScreenBuffer.Current.Handle).Mode;
        
        _command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();

        var currentMode = new Windows.Console(Windows.ScreenBuffer.Current.Handle).Mode;
        currentMode.Should().Be((uint)(mode & ~Windows.Console.EnableWrapAtEolOutput));
    }
}