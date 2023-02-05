using System.Runtime.InteropServices;
using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class ResetColorCommandTest
{
    public static IEnumerable<object[]> Colors
    {
        get
        {
            var colors = Color.AllColors;
            foreach (var color in colors)
            {
                if (color != Color.Reset)
                {
                    yield return new object[] { color };
                }
            }
        }
    }
    
    private readonly ResetColorCommand _command;

    public ResetColorCommandTest()
    {
        _command = new();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void ExecuteWindowsApi_ShouldResetBackgroundColor(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        
        new SetBackgroundColorCommand(color).ExecuteWindowsApi();
        _command.ExecuteWindowsApi();
        
        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;
        attribute.Should().Be(7);
    }
    
    [Theory]
    [MemberData(nameof(Colors))]
    public void ExecuteWindowsApi_ShouldResetForegroundColor(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        
        new SetForegroundColorCommand(color).ExecuteWindowsApi();
        _command.ExecuteWindowsApi();
        
        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;
        attribute.Should().Be(7);
    }
}