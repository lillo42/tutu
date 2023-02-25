namespace Tutu.Windows;

internal static partial class Windows
{
    /// <summary>
    /// The standard handles of a process.
    ///
    /// See [the Windows documentation on console
    /// handles](https://docs.microsoft.com/en-us/windows/console/console-handles) for more info.
    /// </summary>
    public enum HandleType
    {
        /// <summary>
        /// The process' standard output.
        /// </summary>
        OutputHandle,

        /// <summary>
        /// The process' standard input.
        /// </summary>
        InputHandle,

        /// <summary>
        /// The process' active console screen buffer, `CONOUT$`.
        /// </summary>
        CurrentOutputHandle,

        /// <summary>
        /// The process' console input buffer, `CONIN$`.
        /// </summary>
        CurrentInputHandle
    }
}