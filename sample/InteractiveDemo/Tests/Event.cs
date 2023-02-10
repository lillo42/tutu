using Erised.Events;
using Tmds.Linux;
using static Erised.Commands.Cursor;
using static Erised.Commands.Events;

namespace InteractiveDemo.Tests;

public class Event : AbstractTest
{
    public static void TestEvent(TextWriter writer)
    {
        writer.Execute(EnableMouseCapture);

        while (true)
        {
            var @event = EventStream.Default.Read();
            if (@event != null)
            {
                Console.WriteLine(@event);

                if (@event is Erised.Events.Event.KeyEvent { Event.Code: KeyCode.CharKeyCode ch })
                {
                    if (ch.Character == 'c')
                    {
                        var position = Position;
                        Console.WriteLine("Cursor position: ({0}, {1})", position.Column, position.Row);
                    }

                    if (ch.Character == 'q')
                    {
                        break;
                    }
                }
            }
        }

        writer.Execute(DisableMouseCapture);
    }
    
    public static void Run(TextWriter writer)
    {
        Execute(writer, TestEvent);
    }
}