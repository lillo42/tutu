using System.Runtime.InteropServices;
using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Events.Commands;

public class EnableBracketedPasteCommandTest
{
    private readonly EnableBracketedPasteCommand _command;

    public EnableBracketedPasteCommandTest()
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
}