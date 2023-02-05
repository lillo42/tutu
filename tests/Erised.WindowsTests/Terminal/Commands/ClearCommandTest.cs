using System.Runtime.InteropServices;
using Erised.Terminal;
using Erised.Terminal.Commands;
using FluentAssertions;

namespace Erised.WindowsTests.Terminal.Commands;

public class ClearCommandTest
{
    public static IEnumerable<object[]> ClearTypes
    {
        get
        {
            var values = Enum.GetValues(typeof(ClearType));
            foreach (var value in values)
            {
                yield return new[] { value };
            }
        }
    }
    
    [Theory]
    [MemberData(nameof(ClearTypes))]
    public void ExecuteWindowsApi_ShouldClearScreen(ClearType type)
    {
        Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
        var command = new ClearCommand(type);
        command.Invoking(c => c.ExecuteWindowsApi())
            .Should()
            .NotThrow();
    }
}