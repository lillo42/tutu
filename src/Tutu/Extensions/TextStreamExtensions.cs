namespace Tutu.Extensions;

/// <summary>
/// The extensions for <see cref="TextWriter"/>.
/// </summary>
public static class TextStreamExtensions
{
    /// <inheritdoc cref="IExecutableCommand.Execute(Tutu.ICommand)"/>
    public static IExecutableCommand Execute(this TextWriter writer, ICommand command)
    {
        return new ExecutableCommand(writer)
            .Execute(command);
    }

    /// <inheritdoc cref="IExecutableCommand.Execute(Tutu.ICommand[])"/>
    public static IExecutableCommand Execute(this TextWriter writer, params ICommand[] commands)
    {
        return new ExecutableCommand(writer)
            .Execute(commands);
    }


    /// <inheritdoc cref="IQueueExecutor.Enqueue(Tutu.ICommand)"/>
    public static IQueueExecutor Enqueue(this TextWriter writer, ICommand command)
    {
        return new QueueExecutor(writer)
            .Enqueue(command);
    }

    /// <inheritdoc cref="IQueueExecutor.Enqueue(Tutu.ICommand[])"/>
    public static IQueueExecutor Enqueue(this TextWriter writer, params ICommand[] commands)
    {
        return new QueueExecutor(writer)
            .Enqueue(commands);
    }
}
