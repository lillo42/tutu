using System.Runtime.InteropServices;
using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class SetColorsCommandTest
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

    private readonly ResetColorCommand _resetColorCommand;

    public SetColorsCommandTest()
    {
        _resetColorCommand = new();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteForegroundColorAnsiCode(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        
        _resetColorCommand.ExecuteWindowsApi();

        var originalAttributes = Windows.ScreenBuffer.Current.Info.Attributes;

        var command = new SetColorsCommand(color, null);
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();

        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;

        var expectedAttribute =
            (ushort)(Windows.Style.From(Colored.ForegroundColor(color)) | originalAttributes | 0x0070);

        attribute.Should().Be(expectedAttribute);
    }
    
    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteBackgroundColorAnsiCode(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        _resetColorCommand.ExecuteWindowsApi();
        var originalAttributes = Windows.ScreenBuffer.Current.Info.Attributes;

        var command = new SetColorsCommand(null, color);
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();

        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;

        var expectedAttribute =
            (ushort)(Windows.Style.From(Colored.BackgroundColor(color)) | originalAttributes | 0x0070);

        attribute.Should().Be(expectedAttribute);
    }
    
    [Theory]
    [MemberData(nameof(Colors))]
    public void WriteAnsi_ShouldWriteForegroundColorAndBackgroundColorAnsiCode(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        
        _resetColorCommand.ExecuteWindowsApi();
        
        var originalAttributes = Windows.ScreenBuffer.Current.Info.Attributes;

        var command = new SetColorsCommand(color, color);
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();

        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;

        var expectedAttribute =
            (ushort)(Windows.Style.From(Colored.ForegroundColor(color)) | Windows.Style.From(Colored.BackgroundColor(color)) | originalAttributes | 0x0070);

        attribute.Should().Be(expectedAttribute);
    }
}