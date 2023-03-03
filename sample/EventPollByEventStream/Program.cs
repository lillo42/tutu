// Demonstrates how to read events asynchronously.

using NodaTime;
using Tutu.Events;
using Tutu.Extensions;
using static Tutu.Commands.Events;
using Terminal = Tutu.Terminal.Terminal;

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
    await EventStream.Default.StartAsync();

    var reader = EventStream.Default.Reader;

    while (true)
    {
        var source = new CancellationTokenSource();
        try
        {
            source.CancelAfter(Duration.FromSeconds(1).ToTimeSpan());
            var @event = await reader.ReadAsync(source.Token);

            Console.WriteLine(@event);
            if (@event is Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode { Character: 'c' } })
            {
                var position = Tutu.Cursor.Cursor.Position;
                Console.WriteLine("Cursor position: ({0}, {1})", position.Column, position.Row);
            }

            if (@event is Event.KeyEventEvent { Event.Code: KeyCode.EscKeyCode })
            {
                await EventStream.Default.StopAsync();
                break;
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("..");
        }
    }
}
