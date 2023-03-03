using System.Runtime.InteropServices;

namespace Tutu;

/// <summary>
/// The interface for types that can queue commands.
/// </summary>
public interface IQueueExecutor
{
    /// <summary>
    /// Enqueues the given command.
    /// </summary>
    /// <param name="command">The <see cref="ICommand"/>.</param>
    /// <returns>The <see cref="IQueueExecutor"/> with the given command enqueue.</returns>
    IQueueExecutor Enqueue(ICommand command);

    /// <summary>
    /// Enqueues the given commands.
    /// </summary>
    /// <param name="commands">The <see cref="ICommand"/> collection.</param>
    /// <returns>The <see cref="IQueueExecutor"/> with the given command enqueue.</returns>
    IQueueExecutor Enqueue(params ICommand[] commands);

    /// <summary>
    /// Executes all queued commands.
    /// </summary>
    /// <returns>The <see cref="IQueueExecutor"/>.</returns>
    IQueueExecutor Flush();
}

/// <summary>
/// Default implementation of <see cref="IQueueExecutor"/>.
/// </summary>
/// <param name="Writer">The <see cref="TextWriter"/> to execute commands.</param>
public record QueueExecutor(TextWriter Writer) : IQueueExecutor
{
    private readonly Queue<ICommand> _queue = new();

    // Used for testing proposals
    internal int Count => _queue.Count;

    /// <inheritdoc cref="IQueueExecutor.Enqueue(Tutu.ICommand)"/>
    public IQueueExecutor Enqueue(ICommand command)
    {
        _queue.Enqueue(command);
        return this;
    }

    /// <inheritdoc cref="IQueueExecutor.Enqueue(Tutu.ICommand[])"/>
    public IQueueExecutor Enqueue(params ICommand[] commands)
    {
        foreach (var command in commands)
        {
            _queue.Enqueue(command);
        }

        return this;
    }

    /// <inheritdoc cref="IQueueExecutor.Flush"/>
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
