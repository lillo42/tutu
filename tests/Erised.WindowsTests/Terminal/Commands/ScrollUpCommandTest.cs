using AutoFixture;
using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class ScrollUpCommandTest
{
    private readonly Fixture _fixture = new();

    [Fact(Skip = "Need to verify")]
    public void ExecuteWindowsApi_ShouldScrollDownCorrectNumberOfLines()
    {
        var lines = _fixture.Create<ushort>();
        var command = new ScrollUpCommand(lines);

        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}