using NodaTime;

namespace Tutu.Events;

/// <summary>
/// Keeps track of the elapsed time since the moment the polling started.
/// </summary>
public class PollTimeout
{
    private readonly IClock _clock;
    private readonly Instant _start;
    private readonly Duration? _timeout;

    public PollTimeout(IClock clock, Duration? timeout)
    {
        _clock = clock;
        _start = _clock.GetCurrentInstant();
        _timeout = timeout;
    }

    public PollTimeout(Duration? timeout)
        : this(SystemClock.Instance, timeout)
    {
    }

    /// <summary>
    /// Whether the timeout has elapsed.
    /// </summary>
    /// <remarks>
    /// It always returns <see langword="false"/> if the initial timeout was set to <see langword="null"/>.
    /// </remarks>
    public bool Elapsed => _timeout != null && _clock.GetCurrentInstant() - _start >= _timeout.Value;

    /// <summary>
    /// Returns the timeout leftover (initial timeout duration - elapsed duration).
    /// </summary>
    public Duration? Leftover
    {
        get
        {
            if (_timeout == null)
            {
                return _timeout;
            }

            var elapsed = _clock.GetCurrentInstant() - _start;
            if (elapsed >= _timeout)
            {
                return Duration.Zero;
            }

            return _timeout - elapsed;
        }
    }
}