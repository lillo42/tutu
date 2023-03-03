using FluentAssertions;
using Tutu.Terminal.Commands;

namespace Tutu.Tests.Terminal.Commands;

public class LeaveAlternateScreenCommandTest
{
    private readonly LeaveAlternateScreenCommand _command;

    public LeaveAlternateScreenCommandTest()
    {
        _command = new();
    }

    [Fact]
    public void WriteAnsi_ShouldEnterAlternate()
    {
        var writer = new StringWriter();
        _command.WriteAnsi(writer);
        writer.ToString().Should().Be($"{AnsiCodes.CSI}?1049l");
    }
}
