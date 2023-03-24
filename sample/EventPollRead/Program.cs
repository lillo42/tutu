// Demonstrates how to match on modifiers like: Control, alt, shift.

using NodaTime;
using Tutu.Events;
using Tutu.Extensions;
using Tutu.Terminal;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Events;
using static Tutu.Commands.Style;

const string Help = @"Blocking poll() & non-blocking read()
 - Keyboard, mouse and terminal resize events enabled
 - Prints ""."" every second if there's no event
 - Hit ""c"" to print current cursor position
 - Use Esc to quit
";

Console.WriteLine(Help);

SystemTerminal.EnableRawMode();

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
SystemTerminal.DisableRawMode();

void PrintEvents()
{
    while (true)
    {
        if (!SystemEventReader.Poll(Duration.FromSeconds(1)))
        {
            stdout
                .Execute(Print("."))
                .Execute(Print(Environment.NewLine))
                .Execute(MoveToColumn(0));
            continue;
        }

        var @event = SystemEventReader.Read();

        stdout
            .Execute(Print(@event))
            .Execute(Print(Environment.NewLine))
            .Execute(MoveToColumn(0));

        if (@event is Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode { Character: "c" } })
        {
            var position = Tutu.Cursor.SystemCursor.Position;
            stdout
                .Execute(Print($"Cursor position: ({position.Column}, {position.Row})"))
                .Execute(Print(Environment.NewLine))
                .Execute(MoveToColumn(0));
        }

        if (@event is Event.KeyEventEvent { Event.Code: KeyCode.EscKeyCode })
        {
            break;
        }
    }
}
