using FluentAssertions;
using Tutu.Events;

namespace Tutu.Tests.Events;

public class KeyEventTest
{
    [Fact(Skip = "test ")]
    public void Equality()
    {
        var lowercaseAWithShift = new KeyEvent(KeyCode.Char('a'), KeyModifiers.Shift);
        var uppercaseAWithShift = new KeyEvent(KeyCode.Char('A'), KeyModifiers.Shift);
        var uppercaseA = new KeyEvent(KeyCode.Char('A'), KeyModifiers.None);

        /*lowercaseAWithShift.Equals(uppercaseAWithShift).Should().BeTrue();
        uppercaseA.Equals(uppercaseAWithShift).Should().BeTrue();*/
        
        (lowercaseAWithShift == uppercaseAWithShift).Should().BeTrue();
        (uppercaseA == uppercaseAWithShift).Should().BeTrue();
    }
}