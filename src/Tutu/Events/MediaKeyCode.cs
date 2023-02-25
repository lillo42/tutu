namespace Tutu.Events;

/// <summary>
/// Represents a media key (as part of [`KeyCode::Media`]).
/// </summary>
public enum MediaKeyCode
{
    /// <summary>
    /// Play media key.
    /// </summary>
    Play,

    /// <summary>
    /// Pause media key.
    /// </summary>
    Pause,

    /// <summary>
    /// Play/Pause media key.
    /// </summary>
    PlayPause,

    /// <summary>
    /// Reverse media key.
    /// </summary>
    Reverse,

    /// <summary>
    /// Stop media key.
    /// </summary>
    Stop,

    /// <summary>
    /// Fast-forward media key.
    /// </summary>
    FastForward,

    /// <summary>
    /// Rewind media key.
    /// </summary>
    Rewind,

    /// <summary>
    /// Next-track media key.
    /// </summary>
    TrackNext,

    /// <summary>
    /// Previous-track media key.
    /// </summary>
    TrackPrevious,

    /// <summary>
    /// Record media key.
    /// </summary>
    Record,

    /// <summary>
    /// Lower-volume media key.
    /// </summary>
    LowerVolume,

    /// <summary>
    /// Raise-volume media key.
    /// </summary>
    RaiseVolume,

    /// <summary>
    /// Mute media key.
    /// </summary>
    MuteVolume,
}