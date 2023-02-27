using System.Runtime.InteropServices;
using Tutu.Unix2;
using Tutu.Windows;

namespace Tutu.Cursor;

/// <summary>
/// The static instance of <see cref="ICursor"/>. 
/// </summary>
/// <remarks>
/// It's just a wrapper for <see cref="ICursor"/>.
/// Depending on the platform, it will be initialized with a different implementation.
/// For Windows, it will be <see cref="WindowsCursor"/>, for Unix, it will be <see cref="UnixCursor"/>.
/// </remarks>
public static class Cursor
{
    /// <summary>
    /// The current <see cref="ICursor"/> instance.
    /// </summary>
    public static ICursor Instance { get; }

    /// <summary>
    /// Initialize the <see cref="Instance"/>.
    /// </summary>
    static Cursor()
    {
        Instance = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsCursor() : new UnixCursor();
    }
    
    /// <inheritdoc cref="ICursor.Position"/>
    public static CursorPosition Position => Instance.Position;
}