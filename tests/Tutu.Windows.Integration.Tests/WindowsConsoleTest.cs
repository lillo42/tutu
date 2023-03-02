using FluentAssertions;
using Tutu.Style.Types;

namespace Tutu.Windows.Integration.Tests;

public class WindowsConsoleTest
{
    [Fact]
    public void FromBackgroundColor()
    {
        WindowsConsole.FromBackgroundColor(Color.Red)
            .Should().Be(WindowsConsole.BackgroundIntensity | WindowsConsole.BackgroundRed);
    }

    [Fact]
    public void FromForegroundColor()
    {
        WindowsConsole.FromForegroundColor(Color.Red)
            .Should().Be(WindowsConsole.ForegroundIntensity | WindowsConsole.ForegroundRed);
    }

    [Fact]
    public void SetForegroundColor()
    {
        var originalColor = Interlocked.Read(ref WindowsConsole._originalConsoleColor);
        originalColor.Should().Be(uint.MaxValue);
        
        WindowsConsole.CurrentOutput.SetForegroundColor(Color.Red);
        
        originalColor = Interlocked.Read(ref WindowsConsole._originalConsoleColor);
        originalColor.Should().NotBe(uint.MaxValue);
    }
}