using FluentAssertions;
using Tutu.Extensions;
using Tutu.Terminal;
using Tutu.Terminal.Commands;

namespace Tutu.Integration.Tests;

public class TerminalTest
{
    private readonly ITerminal _terminal;

    public TerminalTest()
    {
        _terminal = Tutu.Terminal.SystemTerminal.Instance;
    }

    [Fact]
    public void Resize()
    {
        var (width, height) = _terminal.Size;

        SetSize(35, 35);

        // reset to previous size
        SetSize(width, height);

        void SetSize(int w, int h)
        {
            Console.Out.Execute(new SetSizeCommand((ushort)w, (ushort)h));

            // see issue: https://github.com/eminence/terminal-size/issues/11
            Thread.Sleep(30);

            _terminal.Size.Should().Be(new TerminalSize(w, h));
        }
    }

    [Fact]
    public void RawMode()
    {
        // check we start from normal mode (may fail on some test harnesses)
        _terminal.IsRawModeEnabled.Should().BeFalse();

        // enable the raw mode
        _terminal.EnableRawMode();

        // check it worked (on unix it doesn't really check the underlying
        // tty but rather check that the code is consistent)
        _terminal.IsRawModeEnabled.Should().BeTrue();

        // enable it again, this should not change anything
        _terminal.EnableRawMode();

        // check we're still in raw mode
        _terminal.IsRawModeEnabled.Should().BeTrue();

        // now let's disable it
        _terminal.DisableRawMode();

        // check we're back to normal mode
        _terminal.IsRawModeEnabled.Should().BeFalse();
    }
}
