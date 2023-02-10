using Erised.Events;
using Erised.Terminal;
using static Erised.Commands.Cursor;
using static Erised.Commands.Style;
using static Erised.Commands.Terminal;

namespace InteractiveDemo.Tests;

public abstract class AbstractTest
{
    protected static void Execute(TextWriter writer, params Action<TextWriter>[] actions)
    {
        foreach (var action in actions)
        {
            writer
                .Enqueue(
                    Reset,
                    Clear(ClearType.All),
                    MoveTo(1, 1),
                    Show,
                    EnableBlinking)
                .Flush();

            action(writer);

            writer.Flush();
            
            var ch = ReadChar();
            if (ch == 'q')
            {
                break;
            }
        }
    }

    protected static string SetWidth(string text, int minSize)
    {
        var diff = minSize - text.Length;
        if (diff <= 0)
        {
            return text;
        }

        return new string(' ', diff) + text;
    }

    private static char ReadChar()
    {
        while (true)
        {
            var read = EventStream.Default.Read();
            if (read is Erised.Events.Event.KeyEvent { Event.Code: KeyCode.CharKeyCode ch })
            {
                return ch.Character;
            }
        }
    }
}