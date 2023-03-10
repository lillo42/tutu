namespace Tutu.Cursor;

/// <summary>
/// The cursor information.
/// </summary>
public interface ICursor
{
    /// <summary>
    /// The current cursor position.
    /// </summary>
    CursorPosition Position { get; }
}
