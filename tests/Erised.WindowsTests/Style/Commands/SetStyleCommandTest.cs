using Erised.Style;
using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class SetStyleCommandTest
{
    [Fact]
    public void ExecuteWindowsApi_ShouldThrowNotSupported()
    {
        var command = new SetStyleCommand(ContentStyled.Default.On(Color.Black));
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .Throw<NotSupportedException>();
    }
    
    
    [Fact]
    public void IsAnsiSupported_ShouldReturnTrue()
    {
        var command = new SetStyleCommand(ContentStyled.Default.On(Color.Black));
        ((ICommand)command).IsAnsiCodeSupported.Should().BeTrue();
    }
}