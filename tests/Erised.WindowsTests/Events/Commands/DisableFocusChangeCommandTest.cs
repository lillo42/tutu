using System.Runtime.InteropServices;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class DisableFocusChangeCommandTest
{
    private readonly DisableFocusChangeCommand _command;

    public DisableFocusChangeCommandTest()
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