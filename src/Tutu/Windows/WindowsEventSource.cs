using NodaTime;
using Tutu.Events;
using Tutu.Windows.Interop.Kernel32;

namespace Tutu.Windows;

/// <summary>
/// Windows implementation of <see cref="IEventSource"/>.
/// </summary>
public class WindowsEventSource : IEventSource
{
    private ushort? _surrogate;
    private readonly MouseButtonPressed _mouseButtonPressed = new();
    private readonly WindowsApiPoll _poll = new();
    private readonly WindowsConsole _console = WindowsConsole.CurrentIn;

    /// <inheritdoc />
    public IInternalEvent? TryRead(IClock clock, Duration? timeout)
    {
        var pollTimeout = new PollTimeout(clock, timeout);
        while (true)
        {
            try
            {
                var isEventReady = _poll.Poll(pollTimeout.Leftover);
                if (isEventReady)
                {
                    var number = _console.NumberOfInputEvent;
                    IEvent? @event = null;
                    if (number > 0)
                    {
                        var read = _console.ReadSingleInputEvent();
                        var eventType = (EventType)read.EventType;
                        if (eventType == EventType.KeyEvent)
                        {
                            @event = WindowsEventParse.HandleKeyEvent(read.Event, ref _surrogate);
                        }
                        else if (eventType == EventType.MouseEvent)
                        {
                            @event = WindowsEventParse.HandleMouseEvent(read.Event, _mouseButtonPressed);
                        }
                        else if (eventType == EventType.WindowBufferSizeEvent)
                        {
                            @event = Event.Resize((ushort)(read.Event.dwSize.X + 1),
                                (ushort)(read.Event.dwSize.Y + 1));
                        }
                        else if (eventType == EventType.FocusEvent)
                        {
                            @event = read.Event.bSetFocus ? Event.FocusGained : Event.FocusLost;
                        }
                    }

                    if (@event != null)
                    {
                        return InternalEvent.Event(@event);
                    }
                }
            }
            catch
            {
                // Ignore error
            }

            if (pollTimeout.Elapsed)
            {
                return null;
            }
        }
    }
}