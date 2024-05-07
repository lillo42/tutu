using NodaTime;
using Tutu.Cursor;
using Tutu.Events;
using static Tutu.Events.InternalSystemEventReader;

namespace Tutu.Unix;

/// <summary>
/// The Unix implementation of <see cref="ICursor"/>.
/// </summary>
public sealed class UnixCursor : ICursor
{
    /// <inheritdoc cref="ICursor.Position"/>
    public CursorPosition Position
    {
        get
        {
            var terminal = (UnixTerminal)Terminal.SystemTerminal.Instance;
            if (terminal.IsRawModeEnabled)
            {
                return ReadPositionRaw();
            }

            terminal.EnableRawMode();
            try
            {
                return ReadPositionRaw();
            }
            finally
            {
                terminal.DisableRawMode();
            }
        }
    }

    private static CursorPosition ReadPositionRaw()
    {
        // Use `ESC [ 6 n` to and retrieve the cursor position.
        Console.Out.Write($"{AnsiCodes.CSI}6n");

        while (true)
        {
            try
            {
                var received = PollInternal(Duration.FromSeconds(2), CursorPositionFilter.Default);
                if (received)
                {
                    var read = ReadInternal(CursorPositionFilter.Default);
                    if (read is CursorPositionInternalEvent cursorPositionEvent)
                    {
                        return new CursorPosition(cursorPositionEvent.Column, cursorPositionEvent.Row);
                    }
                    continue;
                }
            }
            catch
            {
                continue;
            }

            throw new IOException("The cursor position could not be read within a normal duration");
        }

    }
}
