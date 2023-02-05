using System.Runtime.InteropServices;
using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class SetBackgroundColorCommandTest
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

    public SetBackgroundColorCommandTest()
    {
        _resetColorCommand = new();
    }

    [Theory]
    [MemberData(nameof(Colors))]
    public void ExecuteWindowsApi_ShouldDoNoting(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        _resetColorCommand.ExecuteWindowsApi();
        
        var originalAttributes = Windows.ScreenBuffer.Current.Info.Attributes;
        
        var command = new SetBackgroundColorCommand(color);

        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
        
        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;
        
        var expectedAttribute = (ushort) (Windows.Style.From(Colored.BackgroundColor(color)) | originalAttributes | 0x0070);
        
        attribute.Should().Be(expectedAttribute);
    }
}