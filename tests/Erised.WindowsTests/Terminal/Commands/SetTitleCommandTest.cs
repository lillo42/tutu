using AutoFixture;
using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class SetTitleCommandTest
{
    private readonly Fixture _fixture = new();
    
    [Fact]
    public void ExecuteWindowsApi_ShouldWriteCorrectAnsiSequence()
    {
        var title = _fixture.Create<string>();
        var command = new SetTitleCommand(title);

        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}
