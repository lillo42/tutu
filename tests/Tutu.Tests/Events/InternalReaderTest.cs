using FluentAssertions;
using NodaTime;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Tutu.Events;

namespace Tutu.Tests.Events;

public class InternalReaderTest
{
    private readonly IEventSource _source;
    private readonly InternalReader _reader;

    public InternalReaderTest()
    {
        _source = Substitute.For<IEventSource>();
        _reader = new InternalReader(_source);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void Poll_Should_ReturnFalse_When_Timeout(int timeout)
    {
        _source.TryRead(Arg.Any<IClock>(), Arg.Any<Duration>())
            .ReturnsNull();

        _reader.Poll(Duration.FromMilliseconds(timeout), PublicEventFilter.Default)
            .Should().BeFalse();

        _source
            .Received()
            .TryRead(Arg.Any<IClock>(), Arg.Any<Duration>());
    }

    [Fact]
    public void Poll_Should_ReturnTrue_When_EventRead()
    {
        var filter = Substitute.For<IFilter>();
        filter.Eval(Arg.Any<IInternalEvent>())
            .Returns(true);

        _source.TryRead(Arg.Any<IClock>(), Arg.Any<Duration>())
            .Returns(Substitute.For<IInternalEvent>());

        _reader.Poll(Duration.FromMilliseconds(10), filter)
            .Should().BeTrue();
    }

    [Fact]
    public void Poll_Should_ReturnTrue_After_MultiEventRead()
    {
        var filter = Substitute.For<IFilter>();
        filter.Eval(Arg.Any<IInternalEvent>())
            .Returns(_ => false, _ => true);

        _source.TryRead(Arg.Any<IClock>(), Arg.Any<Duration>())
            .Returns(Substitute.For<IInternalEvent>());

        _reader.Poll(Duration.FromMilliseconds(10), filter)
            .Should().BeTrue();
    }

    [Fact]
    public void Poll_Should_ReturnTrue_When_EventAlreadyExist()
    {
        var filter = Substitute.For<IFilter>();
        filter.Eval(Arg.Any<IInternalEvent>())
            .Returns(true);

        _source.TryRead(Arg.Any<IClock>(), Arg.Any<Duration>())
            .Returns(Substitute.For<IInternalEvent>());

        _ = _reader.Poll(Duration.FromMilliseconds(10), filter);

        _reader.Poll(Duration.FromMilliseconds(10), filter)
            .Should().BeTrue();
    }

    [Fact]
    public void Read()
    {
        var filter = Substitute.For<IFilter>();
        filter.Eval(Arg.Any<IInternalEvent>())
            .Returns(true);

        _source.TryRead(Arg.Any<IClock>(), Arg.Any<Duration>())
            .Returns(Substitute.For<IInternalEvent>());

        var @event = _reader.Read(filter);
        @event.Should().NotBeNull();
    }

    [Fact]
    public void Read_Should_CallTryReadMultipleTime()
    {
        var filter = Substitute.For<IFilter>();
        filter.Eval(Arg.Any<IInternalEvent>())
            .Returns(_ => false, _ => false, _ => true, _ => true);

        _source.TryRead(Arg.Any<IClock>(), Arg.Any<Duration>())
            .Returns(Substitute.For<IInternalEvent>());

        var @event = _reader.Read(filter);
        @event.Should().NotBeNull();
    }
}