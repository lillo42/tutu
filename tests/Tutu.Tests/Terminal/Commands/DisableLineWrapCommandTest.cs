using FluentAssertions;
using Tutu.Terminal.Commands;

namespace Tutu.Tests.Terminal.Commands;

public class DisableLineWrapCommandTest
{
    private readonly DisableLineWrapCommand _command;

    public DisableLineWrapCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldDisableLine()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}?7l");
    }
}