using System.Runtime.InteropServices;
using Tutu.Unix2;
using Tutu.Windows;

namespace Tutu;

/// <summary>
/// Indicates whether the current platform supports ANSI escape codes.
/// </summary>
public interface IAnsiSupport
{
    /// <summary>
    /// Whether the ANSI code representation of this command is supported.
    /// </summary>
    /// <remarks>
    /// On Unix systems, this is always true.
    ///
    /// On Windows systems, this is true if the current version is Windows 10 or higher.
    /// 
    /// A list of supported ANSI escape codes can be found <see href="https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences">here</see>
    /// </remarks>
    bool IsAnsiSupported { get; }
}

/// <summary>
/// Single instance of <see cref="IAnsiSupport"/>.
/// </summary>
public static class AnsiSupport
{
    /// <summary>
    /// The singleton instance of <see cref="AnsiSupport"/>.
    /// </summary>
    public static IAnsiSupport Instance { get; }

    /// <summary>
    /// static constructor.
    /// </summary>
    static AnsiSupport()
    {
        Instance = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new WindowsAnsiSupport()
            : new UnixAnsiSupport();
    }

    /// <inheritdoc cref="IAnsiSupport.IsAnsiSupported" />.
    public static bool IsAnsiSupported => Instance.IsAnsiSupported;
}