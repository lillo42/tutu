using System.Runtime.InteropServices;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal;

public class TerminalTest
{
    [Fact(Skip = "Need to verify")]
    public void EnableRawMode()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        Erised.Terminal.Terminal.EnableRawMode();

        Erised.Terminal.Terminal.IsRawModeEnable.Should().BeTrue();

        Erised.Terminal.Terminal.DisableRawMode();
    }
    
    [Fact(Skip = "Need to verify")]
    public void DisableRawMode()
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        Erised.Terminal.Terminal.EnableRawMode();
        
        Erised.Terminal.Terminal.DisableRawMode();

        Erised.Terminal.Terminal.IsRawModeEnable.Should().BeFalse();
    }
}