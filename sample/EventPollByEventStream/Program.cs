// Demonstrates how to read events asynchronously.

using NodaTime;
using Tutu.Events;
using Tutu.Extensions;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Events;
using static Tutu.Commands.Style;
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

async Task PrintEventsAsync()
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

            stdout
                .Execute(Print(@event))
                .Execute(Print(Environment.NewLine))
                .Execute(MoveToColumn(0));

            if (@event is Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode { Character: "c" } })
            {
                var position = Tutu.Cursor.Cursor.Position;
                stdout
                    .Execute(Print($"Cursor position: ({position.Column}, {position.Row})"))
                    .Execute(Print(Environment.NewLine))
                    .Execute(MoveToColumn(0));
            }

            if (@event is Event.KeyEventEvent { Event.Code: KeyCode.EscKeyCode })
            {
                await EventStream.Default.StopAsync();
                break;
            }
        }
        catch (OperationCanceledException)
        {
            stdout
                .Execute(Print("."))
                .Execute(Print(Environment.NewLine))
                .Execute(MoveToColumn(0));
        }
    }
}
