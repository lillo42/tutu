using System.Runtime.InteropServices;
using Tutu.Unix2;
using Tutu.Windows;

namespace Tutu.Terminal;

/// <summary>
/// The static instance of <see cref="ITerminal"/>.
/// </summary>
/// <remarks>
/// It's a wrapper for <see cref="ITerminal"/>.
/// Depending on the platform, it will be initialized with a different implementation.
/// for Windows, it will be <see cref="WindowsTerminal"/>, for Unix, it will be <see cref="UnixTerminal"/>.
/// </remarks>
public static class Terminal
{
    /// <summary>
    /// The current <see cref="ITerminal"/> instance.
    /// </summary>
    public static ITerminal Instance { get; }

    /// <summary>
    /// Initialize the terminal.
    /// </summary>
    static Terminal()
    {
        Instance = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsTerminal() : new UnixTerminal();
    }

    /// <inheritdoc cref="ITerminal.IsRawModeEnabled"/> 
    public static bool IsRawModeEnable => Instance.IsRawModeEnabled;

    /// <inheritdoc cref="ITerminal.EnableRawMode"/> 
    public static void EnableRawMode() => Instance.EnableRawMode();

    /// <inheritdoc cref="ITerminal.DisableRawMode"/> 
    public static void DisableRawMode() => Instance.DisableRawMode();

    /// <inheritdoc cref="ITerminal.Size"/> 
    public static TerminalSize Size => Instance.Size;

    /// <inheritdoc cref="ITerminal.SupportsKeyboardEnhancement"/> 
    public static bool SupportsKeyboardEnhancement => Instance.SupportsKeyboardEnhancement;
}