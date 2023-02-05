using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class EnterAlternateScreenCommandTest
{
    private readonly EnterAlternateScreenCommand _command;

    public EnterAlternateScreenCommandTest()
    {
        _command = new();
    }
    
    [Fact]
    public void ExecuteWindowsApi_ShouldEnterAlternate()
    {
        _command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}