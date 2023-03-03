// Demonstrates how to match on modifiers like: Control, alt, shift.

using NodaTime;
using Tutu.Events;
using Tutu.Extensions;
using static Tutu.Commands.Events;
using Terminal = Tutu.Terminal.Terminal;

const string Help = @"Blocking poll() & non-blocking read()
 - Keyboard, mouse and terminal resize events enabled
 - Prints ""."" every second if there's no event
 - Hit ""c"" to print current cursor position
 - Use Esc to quit
";

Console.WriteLine(Help);

Terminal.EnableRawMode();
var stdout = Console.Out;
stdout.Execute(EnableMouseCapture);

try
{
    PrintEvents();
}
catch (Exception e)
{
    Console.WriteLine("Error: {0}", e);
}

stdout.Execute(DisableMouseCapture);
Terminal.DisableRawMode();

static void PrintEvents()
{
    while (true)
    {
        var isPolled = EventReader.Poll(Duration.FromSeconds(1));
        if (!isPolled)
        {
            Console.WriteLine("..");
            continue;
        }

        var @event = EventReader.Read();
        Console.WriteLine(@event);
        if (@event is Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode { Character: 'c' } })
        {
            var position = Tutu.Cursor.Cursor.Position;
            Console.WriteLine("Cursor position: ({0}, {1})", position.Column, position.Row);
        }

        if (@event is Event.KeyEventEvent { Event.Code: KeyCode.EscKeyCode })
        {
            break;
        }
    }
}
