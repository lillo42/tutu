using Tutu.Terminal;
using Console = Tutu.Windows.Windows.Console;

namespace Tutu.Windows2;

/// <summary>
/// The Windows implementation of <see cref="ITerminal"/>.
/// </summary>
/// <remarks>
/// It should be use as Singleton.
/// </remarks>
public class WindowsTerminal : ITerminal
{
    private const uint EnableLineInput = 0x0002;
    private const uint EnableEchoInput = 0x0004;
    private const uint EnableProcessedInput = 0x0001;
    private const uint NotRawModeMask = EnableLineInput | EnableEchoInput | EnableProcessedInput;

    /// <inheritdoc cref="ITerminal.IsRawModeEnabled"/> 
    public bool IsRawModeEnabled => (Console.Input.Mode & NotRawModeMask) == 0;

    /// <inheritdoc cref="ITerminal.DisableRawMode"/> 
    public void EnableRawMode()
    {
        var console = Console.Input;
        console.Mode &= ~NotRawModeMask;
    }

    /// <inheritdoc cref="ITerminal.DisableRawMode"/> 
    public void DisableRawMode()
    {
        var console = Console.Input;
        console.Mode |= NotRawModeMask;
    }

    /// <inheritdoc cref="ITerminal.Size"/> 
    public TerminalSize Size 
    {
        get
        {
            var size = Windows.Windows.ScreenBuffer.Current.Info.TerminalSize;
            return new(size.Width + 1, size.Height + 1);
        } 
    }

    /// <inheritdoc cref="ITerminal.SupportsKeyboardEnhancement"/> 
    public bool SupportsKeyboardEnhancement => false;
}