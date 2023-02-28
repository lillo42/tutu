using Tutu.Events;
using Tmds.Linux;
using Tutu.Extensions;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Events;

namespace InteractiveDemo.Tests;

public class Event : AbstractTest
{
    public static void TestEvent(TextWriter writer)
    {
        writer.Execute(EnableMouseCapture);

        while (true)
        {
            var @event = EventReader.Read();
            Console.WriteLine(@event);

            if (@event is Tutu.Events.Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode ch } keyEvent)
            {
                if (ch.Character == 'c')
                {
                    var position = Tutu.Cursor.Cursor.Position;
                    Console.WriteLine("Cursor position: ({0}, {1})", position.Column, position.Row);
                }

                if (ch.Character == 'q')
                {
                    break;
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