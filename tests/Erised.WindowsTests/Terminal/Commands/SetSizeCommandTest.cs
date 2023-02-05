using AutoFixture;
using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class SetSizeCommandTest
{
    private readonly Fixture _fixture = new();
    
    [Fact(Skip = "Need to verify")]
    public void WriteAnsi_ShouldWriteCorrectAnsiSequence()
    {
        var width = _fixture.Create<ushort>();
        var height = _fixture.Create<ushort>();
        var command = new SetSizeCommand(width, height);
        command.Invoking(c  => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}