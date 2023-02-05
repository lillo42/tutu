using System.Runtime.InteropServices;
using Erised.Style.Commands;
using Erised.Style.Types;
using FluentAssertions;

namespace Erised.WindowsTests.Style.Commands;

public class SetForegroundColorCommandTest
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


    [Theory]
    [MemberData(nameof(Colors))]
    public void ExecuteWindowsApi_ShouldDoNoting(Color color)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

        var originalAttributes = Windows.ScreenBuffer.Current.Info.Attributes;

        var command = new SetForegroundColorCommand(color);

        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();

        var screenBuffer = Windows.ScreenBuffer.Current;
        var attribute = screenBuffer.Info.Attributes;

        var expectedAttribute =
            (ushort)(Windows.Style.From(Colored.ForegroundColor(color)) | originalAttributes | 0x0070);

        attribute.Should().Be(expectedAttribute);
    }
}