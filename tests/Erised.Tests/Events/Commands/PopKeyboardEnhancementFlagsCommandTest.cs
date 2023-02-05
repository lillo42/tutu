using Erised.Events.Commands;
using FluentAssertions;

namespace Erised.Tests.Events.Commands;

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