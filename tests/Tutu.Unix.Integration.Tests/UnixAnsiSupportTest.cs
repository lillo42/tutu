using FluentAssertions;

namespace Tutu.Unix.Integration.Tests;

public class UnixAnsiSupportTest
{
    [Fact]
    public void IsAnsiSupported_Should_BeTrue()
    {
        var ansi = new UnixAnsiSupport();
        ansi.IsAnsiSupported.Should().BeTrue();
    }
}
