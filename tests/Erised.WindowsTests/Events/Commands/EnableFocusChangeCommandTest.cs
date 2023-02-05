using System.Runtime.InteropServices;
using Erised.Events;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class EnableFocusChangeCommandTest
{
    private readonly EnableFocusChangeCommand _command;

    public EnableFocusChangeCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldDoNothing()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        _command
            .Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}