using System.Runtime.InteropServices;
using AutoFixture;
using Erised.Style.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class PrintCommandTest
{
    private readonly Fixture _fixture;

    public PrintCommandTest()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void ExecuteWindowsApi_ShouldThrowNotSupportedException()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        var content = _fixture.Create<string>();
        new PrintCommand<string>(content)
            .Invoking(x => x.ExecuteWindowsApi())
            .Should()
            .Throw<NotSupportedException>();
    }

    [Fact]
    public void IsAnsiSupported_ShouldReturnTrue()
    {
        var content = _fixture.Create<string>();
        ((ICommand)new PrintCommand<string>(content))
            .IsAnsiCodeSupported
            .Should()
            .BeTrue();
    }
}