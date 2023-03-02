using FluentAssertions;
using NSubstitute;
using Tutu.Events;

namespace Tutu.Tests.Events;

public class FilterTest
{
    [Fact]
    public void PublicFilter_Should_ReturnTrue_When_PublicEvent()
    {
        PublicEventFilter.Default.Eval(new PublicEvent(Substitute.For<IEvent>()))
            .Should().BeTrue();
    }

    [Fact]
    public void PublicFilter_Should_ReturnFalse_When_IsNotPublicEvent()
    {
        PublicEventFilter.Default.Eval(new PrimaryDeviceAttributesInternalEvent())
            .Should().BeFalse();
    }

    [Fact]
    public void CursorPositionFilter_Should_ReturnTrue_When_CursorPositionEvent()
    {
        CursorPositionFilter.Default.Eval(new CursorPositionInternalEvent(0, 0))
            .Should().BeTrue();
    }

    [Fact]
    public void CursorPositionFilter_Should_ReturnFalse_When_IsNotCursorPositionEvent()
    {
        CursorPositionFilter.Default.Eval(new PrimaryDeviceAttributesInternalEvent())
            .Should().BeFalse();
    }

    [Fact]
    public void KeyboardEnhancementFlagsFilter_Should_ReturnTrue_When_KeyboardEnhancementFlags()
    {
        KeyboardEnhancementFlagsFilter.Default
            .Eval(new KeyboardEnhancementFlagsInternalEvent(KeyboardEnhancementFlags.None))
            .Should().BeTrue();
    }

    [Fact]
    public void KeyboardEnhancementFlagsFilter_Should_ReturnFalse_When_IsNotKeyboardEnhancementFlags()
    {
        KeyboardEnhancementFlagsFilter.Default.Eval(new PrimaryDeviceAttributesInternalEvent())
            .Should().BeFalse();
    }
    
    [Fact]
    public void PrimaryDeviceAttributesFilter_Should_ReturnTrue_When_PrimaryDeviceAttributes()
    {
        PrimaryDeviceAttributesFilter.Default
            .Eval(new PrimaryDeviceAttributesInternalEvent())
            .Should().BeTrue();
    }
    
    [Fact]
    public void PrimaryDeviceAttributesFilter_Should_ReturnFalse_When_IsNotPrimaryDeviceAttributes()
    {
        PrimaryDeviceAttributesFilter.Default.Eval(new CursorPositionInternalEvent(0, 0))
            .Should().BeFalse();
    }
}