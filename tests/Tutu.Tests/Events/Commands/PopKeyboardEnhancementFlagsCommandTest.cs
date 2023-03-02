using FluentAssertions;
using Tutu.Events.Commands;

namespace Tutu.Tests.Events.Commands;

public class PopKeyboardEnhancementFlagsCommandTest
{
    private readonly PopKeyboardEnhancementFlagsCommand _command;

    public PopKeyboardEnhancementFlagsCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldDoThrowNotSupported()
        => _command
            .Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .Throw<NotSupportedException>();
}