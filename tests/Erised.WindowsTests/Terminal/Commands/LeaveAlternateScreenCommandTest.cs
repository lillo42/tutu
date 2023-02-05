using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class LeaveAlternateScreenCommandTest
{
    private readonly LeaveAlternateScreenCommand _command;

    public LeaveAlternateScreenCommandTest()
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