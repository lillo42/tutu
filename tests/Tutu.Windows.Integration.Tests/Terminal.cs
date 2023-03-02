using System.Runtime.InteropServices;
using FluentAssertions;

namespace Tutu.Windows.Integration.Tests;

public class Terminal
{
    [Fact]
    public void ScrollDownWinApi()
    {
        var currentWindows = ScreenBuffer.CurrentOutput.Info.TerminalWindow;
        
        WindowsTerminal.ScrollDown(2);
        
        var newWindows = ScreenBuffer.CurrentOutput.Info.TerminalWindow;
        newWindows.Top.Should().Be((short)(currentWindows.Top + 2));
        newWindows.Bottom.Should().Be((short)(currentWindows.Bottom + 2));
    }
    
    [Fact]
    public void ScrollUpWinApi()
    {
        var currentWindows = ScreenBuffer.CurrentOutput.Info.TerminalWindow;
        
        WindowsTerminal.ScrollUp(2);
        
        var newWindows = ScreenBuffer.CurrentOutput.Info.TerminalWindow;
        newWindows.Top.Should().Be((short)(currentWindows.Top - 2));
        newWindows.Bottom.Should().Be((short)(currentWindows.Bottom - 2));
    }
    
    
    [Fact]
    public void SetTitleWinApi()
    { 
        const string testTitle = "this is a tutu test title";
        WindowsTerminal.SetTitle(testTitle);

#pragma warning disable CA1416
        Console.Title.Should().Be(testTitle);
#pragma warning restore CA1416
    }
}