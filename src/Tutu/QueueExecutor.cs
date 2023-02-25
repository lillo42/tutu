using System.Runtime.InteropServices;

namespace Tutu;

public interface IQueueExecutor
{
    IQueueExecutor Enqueue(ICommand command);

    IQueueExecutor Enqueue(params ICommand[] commands);

    IQueueExecutor Flush();
}

public record QueueExecutor(TextWriter Writer) : IQueueExecutor
{
    private readonly Queue<ICommand> _queue = new();

    // Used for testing proposals
    internal int Count => _queue.Count;

    public IQueueExecutor Enqueue(ICommand command)
    {
        _queue.Enqueue(command);
        return this;
    }

    public IQueueExecutor Enqueue(params ICommand[] commands)
    {
        foreach (var command in commands)
        {
            _queue.Enqueue(command);
        }

        return this;
    }

    public IQueueExecutor Flush()
    {
        while (_queue.TryDequeue(out var command))
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !command.IsAnsiCodeSupported)
            {
                // There may be queued commands in this writer, but `execute_winapi` will execute the
                // command immediately. To prevent commands being executed out of order we flush the
                // writer now.
                Writer.Flush();
                command.ExecuteWindowsApi();
            }
            else
            {
                command.WriteAnsi(Writer);
            }
        }

        Writer.Flush();
        return this;
    }
}