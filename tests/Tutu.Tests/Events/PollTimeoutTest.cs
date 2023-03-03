using FluentAssertions;
using NodaTime;
using NSubstitute;
using Tutu.Events;

namespace Tutu.Tests.Events;

public class PollTimeoutTest
{
    [Fact]
    public void Leftover_Should_BeNull_When_TimeoutIsNull()
    {
        var timeout = new PollTimeout(null);
        timeout.Leftover.Should().BeNull();
    }

    [Fact]
    public void Elapsed_Should_BeFalse_When_TimeoutIsNull()
    {
        var timeout = new PollTimeout(null);
        timeout.Elapsed.Should().BeFalse();
    }

    [Fact]
    public void Elapsed_Should_BeTrue_When_ItIsElapsed()
    {
        const int timeout = 100;

        var clock = Substitute.For<IClock>();

        clock.GetCurrentInstant()
            .Returns(
                _ => SystemClock.Instance.GetCurrentInstant() - Duration.FromMilliseconds(2 * timeout),
                _ => SystemClock.Instance.GetCurrentInstant(),
                _ => SystemClock.Instance.GetCurrentInstant());

        var pollTimeout = new PollTimeout(clock, Duration.FromMilliseconds(timeout));
        pollTimeout.Elapsed.Should().BeTrue();
        pollTimeout.Leftover.Should().Be(Duration.Zero);
    }

    [Fact]
    public void Elapsed_Should_BeFalse_When_ItIsNotElapsed()
    {
        var pollTimeout = new PollTimeout(Duration.FromSeconds(60));
        pollTimeout.Elapsed.Should().BeFalse();
        (pollTimeout.Leftover > Duration.Zero).Should().BeTrue();
    }
}
