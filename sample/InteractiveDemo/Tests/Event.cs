using Tutu.Events;
using Tutu.Extensions;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Events;
using static Tutu.Commands.Style;

namespace InteractiveDemo.Tests;

public class Event : AbstractTest
{
    public static void TestEvent(TextWriter writer)
    {
        writer.Execute(EnableMouseCapture);

        var stdout = Console.Out;
        while (true)
        {
            var @event = EventReader.Read();

            stdout
                .Execute(Print(@event))
                .Execute(Print(Environment.NewLine))
                .Execute(MoveToColumn(0));

            if (@event is Tutu.Events.Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode ch })
            {
                if (ch.Character[0] == 'c')
                {
                    var position = Tutu.Cursor.Cursor.Position;
                    stdout
                        .Execute(Print($"Cursor position: ({position.Column}, {position.Row})"))
                        .Execute(Print(Environment.NewLine))
                        .Execute(MoveToColumn(0));
                }

                if (ch.Character[0] == 'q')
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
