using System.Runtime.InteropServices;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class DisableBracketedPasteCommandTest
{
    private readonly DisableBracketedPasteCommand _command;

    public DisableBracketedPasteCommandTest()
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