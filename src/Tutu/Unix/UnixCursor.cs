using NodaTime;
using Tutu.Cursor;
using Tutu.Events;
using static Tutu.Events.DefaultEventReader;

namespace Tutu.Unix;

/// <summary>
/// The Unix implementation of <see cref="ICursor"/>.
/// </summary>
public class UnixCursor : ICursor
{
    /// <inheritdoc cref="ICursor.Position"/>
    public CursorPosition Position
    {
        get
        {
            var terminal = (UnixTerminal)Terminal.Terminal.Instance;
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
        /*
        var stdout = Console.OpenStandardOutput();
        stdout.Write("\x1B[6n"u8);
        stdout.Flush();
        */
        
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
