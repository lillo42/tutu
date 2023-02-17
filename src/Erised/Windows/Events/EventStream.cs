using System.Runtime.InteropServices;
using System.Threading.Channels;
using Erised.Events;
using NodaTime;
using static Erised.Windows.Interop.Kernel32;

namespace Erised;

internal static partial class Windows
{
    public partial class WindowsEventStream
    {
        private readonly Channel<IEvent> _channel;

        public WindowsEventStream(int capacity)
        {
            _channel = Channel.CreateBounded<IEvent>(new BoundedChannelOptions(capacity)
            {
                SingleReader = false,
                SingleWriter = true,
                AllowSynchronousContinuations = false
            });
        }

        public ChannelReader<IEvent> Reader => _channel.Reader;

        private ushort? _suggorate;
        private MouseButtonPressed _mouseButtonPressed;

        public async Task ConsumeConsoleEventAsync(
            IClock clock,
            Duration? duration,
            Action? onTimeout,
            CancellationToken cancellationToken)
        {
            await Task.Yield();

            var writer = _channel.Writer;
            var console = Console.CurrentIn;
            while (!cancellationToken.IsCancellationRequested)
            {
                var startedAt = clock.GetCurrentInstant();
                var eventReady = WaitEvent(console.Handle, duration);
                if (eventReady)
                {
                    var inputs = console.ReadMultipleInput();
                    foreach (var input in inputs)
                    {
                        var parse = Parse(input, ref _suggorate);
                        if (parse != null)
                        {
                            await writer.WriteAsync(parse, cancellationToken);
                        }
                    }
                }

                if (!cancellationToken.IsCancellationRequested &&
                    IsElapsed(startedAt, clock.GetCurrentInstant(), duration))
                {
                    onTimeout?.Invoke();
                }
            }
        }

        public IEvent? Read(IClock clock, Duration? duration, Action? onTimeout)
        {
            var console = Console.CurrentIn;
            var startedAt = clock.GetCurrentInstant();
            var eventReady = WaitEvent(console.Handle, duration);
            if (eventReady)
            {
                var input = console.ReadSingleInputEvent();
                var parse = Parse(input, ref _suggorate);
                if (parse != null)
                {
                    return parse;
                }
            }

            if (IsElapsed(startedAt, clock.GetCurrentInstant(), duration))
            {
                onTimeout?.Invoke();
            }

            return null;
        }

        private static bool IsElapsed(Instant start, Instant now, Duration? duration)
        {
            if (duration == null)
            {
                return false;
            }

            var elapsed = now - start;
            return elapsed >= duration.Value;
        }

        private static bool WaitEvent(Handle handle, Duration? timeout)
        {
            var dwMilliseconds = 0xFFFFFFFF;
            if (timeout != null)
            {
                dwMilliseconds = (uint)timeout.Value.TotalMilliseconds;
            }

            var output = WaitForSingleObject(handle, dwMilliseconds);
            if (output == WAIT_OBJECT_0)
            {
                return true;
            }

            if (output == WAIT_OBJECT_0 + 1)
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }

            if (output is WAIT_TIMEOUT or WAIT_ABANDONED)
            {
                return false;
            }

            if (output == WAIT_FAILED)
            {
                throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
            }

            throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
        }

        private IEvent? Parse(INPUT_RECORD @event, ref ushort? surrogate)
        {
            var eventType = (EventType)@event.EventType;

            if (eventType == EventType.KeyEvent)
            {
                var key = HandleKeyEvent(@event.Event, ref surrogate);
                if (key != null)
                {
                    return key;
                }
            }

            if (eventType == EventType.MouseEvent)
            {
                var mouse = HandleMouseEvent(@event.Event, ref _mouseButtonPressed);
                if (mouse != null)
                {
                    return mouse;
                }
            }

            if (eventType == EventType.FocusEvent)
            {
                return @event.Event.bSetFocus ? Erised.Events.Event.FocusGained : Erised.Events.Event.FocusLost;
            }

            if (eventType == EventType.WindowBufferSizeEvent)
            {
                return Erised.Events.Event.Resize(
                    (ushort)(@event.Event.dwSize.X + 1),
                    (ushort)(@event.Event.dwSize.Y + 1));
            }

            return null;
        }
    }
}