namespace Tutu.Extensions;

public static class TextStreamExtensions
{
    public static IExecutableCommand Execute(this TextWriter writer, ICommand command)
    {
        return new ExecutableCommand(writer)
            .Execute(command);
    }
    
    public static IExecutableCommand Execute(this TextWriter writer, params ICommand[] commands)
    {
        return new ExecutableCommand(writer)
            .Execute(commands);
    }
    
    
    public static IQueueExecutor Enqueue(this TextWriter writer, ICommand command)
    {
        return new QueueExecutor(writer)
            .Enqueue(command);
    }

    public static IQueueExecutor Enqueue(this TextWriter writer, params ICommand[] commands)
    {
        return new QueueExecutor(writer)
            .Enqueue(commands);
    }
}