// Demonstrates how to read events asynchronously.

using Erised.Commands;
using Erised.Events;
using NodaTime;
using static Erised.Commands.Events;
using Terminal = Erised.Terminal.Terminal;

const string Help = @"Event Stream for read Events
 - Keyboard, mouse and terminal resize events enabled
 - Prints ""."" every second if there's no event
 - Hit ""c"" to print current cursor position
 - Use Esc to quit
";

Console.WriteLine(Help);

Terminal.EnableRawMode();
var stdout = Console.Out;
stdout.Execute(EnableMouseCapture);

await PrintEventsAsync();

stdout.Execute(DisableMouseCapture);
Terminal.DisableRawMode();

static async Task PrintEventsAsync()
{
    EventStream.Instance.Start(
        Duration.FromSeconds(1),
        () => Console.WriteLine("."));
    var reader = EventStream.Instance.Reader;
    while (await reader.WaitToReadAsync())
    {
        var @event = await reader.ReadAsync();
        
        Console.WriteLine(@event);
        if (@event is Event.KeyEvent { Event.Code: KeyCode.CharKeyCode { Character: 'c' } })
        {
            var position = Cursor.Position;
            Console.WriteLine("Cursor position: ({0}, {1})", position.Column, position.Row);
        }

        if (@event is Event.KeyEvent { Event.Code: KeyCode.EscKeyCode })
        {
            EventStream.Instance.Stop();
            break;
        }
    }
}
