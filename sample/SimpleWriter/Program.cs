// This shows how an application can write on stderr
// instead of stdout, thus making it possible to
// the command API instead of the "old style" direct
// unbuffered API.

using Erised.Events;
using static Erised.Commands.Cursor;
using static Erised.Commands.Style;
using static Erised.Commands.Terminal;

const string Text = @"
This screen is ran on stderr.
And when you hit enter, it prints on stdout.
This makes it possible to run an application and choose what will
be sent to any application calling yours.

what the application prints on stdout is used as argument to cd.

Try it out.

Hit any key to quit this screen:

1 will print `..`
2 will print `/`
3 will print `~`
Any other key will print this text (so that you may copy-paste)
";

var queue = Console.Error
    .Enqueue(EnterAlternateScreen)
    .Enqueue(Hide)
    .Flush();

var y = 1;
foreach (var line in Text.Split('\n'))
{
    queue
        .Enqueue(MoveTo(1, (ushort)y))
        .Enqueue(Print(line))
        .Flush();
    y += 1;
}

Erised.Terminal.Terminal.EnableRawMode();

var key = ReadChar();

Console.Error
    .Execute(Show)
    .Execute(LeaveAlternateScreen);

Erised.Terminal.Terminal.EnableRawMode();


if (key == '1')
{
    Console.WriteLine("..");
}
else if (key == '2')
{
    Console.WriteLine("/");
}
else if (key == '3')
{
    Console.WriteLine("~");
}
else
{
    Console.WriteLine(Text);
}

static char ReadChar()
{
    while (true)
    {
        var @event = EventStream.Instance.Read();
        if (@event is Event.KeyEvent { Event.Code: KeyCode.CharKeyCode ch })
        {
            return ch.Character;
        }
    }
}