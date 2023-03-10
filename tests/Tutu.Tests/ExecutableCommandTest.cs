using FluentAssertions;
using NSubstitute;

namespace Tutu.Tests;

public class ExecutableCommandTest
{
    private readonly StringWriter _writer;
    private readonly ExecutableCommand _executableCommand;

    public ExecutableCommandTest()
    {
        _writer = new();
        _executableCommand = new(_writer);
    }

    [Fact]
    public void Execute()
    {
        var command1 = Substitute.For<ICommand>();
        command1.IsAnsiCodeSupported.Returns(true);
        command1.When(c => c.WriteAnsi(Arg.Any<TextWriter>()))
            .Do(x => _writer.Write("1"));

        _executableCommand.Execute(command1);

        _writer.ToString().Should().Be("1");
    }

    [Fact]
    public void Execute_Multiple()
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

        _executableCommand.Execute(command1, command2, command3);
        _writer.ToString().Should().Be("123");
    }
}
