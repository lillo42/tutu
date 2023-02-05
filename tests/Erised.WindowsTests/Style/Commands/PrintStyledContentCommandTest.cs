using System.Runtime.InteropServices;
using AutoFixture;
using Erised.Style;
using Erised.Style.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class PrintStyledContentCommandTest
{
    private readonly Fixture _fixture;

    public PrintStyledContentCommandTest()
    {
        _fixture = new();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldDoNothing()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        var command = new PrintStyledContentCommand<string>(
            new StyledContent<string>(ContentStyled.Default,
                _fixture.Create<string>()));

        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}