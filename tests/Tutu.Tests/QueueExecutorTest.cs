using FluentAssertions;
using NSubstitute;

namespace Tutu.Tests;

public class QueueExecutorTest
{
    private readonly StringWriter _writer;
    private readonly QueueExecutor _queueExecutor;

    public QueueExecutorTest()
    {
        _writer = new StringWriter();
        _queueExecutor = new QueueExecutor(_writer);
    }


    [Fact]
    public void Enqueue_ShouldNotExecute()
    {
        _queueExecutor.Enqueue(Substitute.For<ICommand>());
        _queueExecutor.Count.Should().Be(1);
    }

    [Fact]
    public void EnqueueMulti_ShouldNotExecute()
    {
        _queueExecutor.Enqueue(
            Substitute.For<ICommand>(),
            Substitute.For<ICommand>(),
            Substitute.For<ICommand>());

        _queueExecutor.Count.Should().Be(3);
    }

    [Fact]
    public void Enqueue_ShouldExecute()
    {
        var command = Substitute.For<ICommand>();
        command.IsAnsiCodeSupported.Returns(true);
        _queueExecutor.Enqueue(command);
        _queueExecutor.Flush();
        _queueExecutor.Count.Should().Be(0);
    }

    [Fact]
    public void Flush()
    {
        var command1 = Substitute.For<ICommand>();
        command1.IsAnsiCodeSupported.Returns(true);
        command1.When(c => c.WriteAnsi(Arg.Any<TextWriter>()))
            .Do(x => _writer.Write("1"));

        var command2 = Substitute.For<ICommand>();
        command2.IsAnsiCodeSupported.Returns(true);
        command2.When(c => c.WriteAnsi(Arg.Any<TextWriter>()))
            .Do(x => _writer.Write("2"));

        var command3 = Substitute.For<ICommand>();
        command3.IsAnsiCodeSupported.Returns(true);
        command3.When(c => c.WriteAnsi(Arg.Any<TextWriter>()))
            .Do(x => _writer.Write("3"));

        _queueExecutor.Enqueue(command1, command2, command3);
        _queueExecutor.Flush();
        _queueExecutor.Count.Should().Be(0);
        _writer.ToString().Should().Be("123");
    }
}
