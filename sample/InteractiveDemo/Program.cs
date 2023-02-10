using Erised.Events;
using Erised.Terminal;
using static Erised.Commands.Cursor;
using static Erised.Commands.Style;
using static Erised.Commands.Terminal;
using Terminal = Erised.Terminal.Terminal;

const string Menu = @"Erised interactive test

Controls:

 - 'q' - quit interactive test (or return to this menu)
 - any other key - continue with next step

Available tests:

1. cursor
2. color (foreground, background)
3. attributes (bold, italic, ...)
4. input

Select test to run ('1', '2', ...) or hit 'q' to quit.
";


var stdout = Console.Out;

stdout.Execute(EnterAlternateScreen);

Terminal.EnableRawMode();


while (true)
{
    
    var queue = stdout
        .Enqueue(
            Reset,
            Clear(ClearType.All),
            Hide,
            MoveTo(1, 1));

    foreach (var line in Menu.Split('\n'))
    {
        queue
            .Enqueue(Print(line))
            .Enqueue(MoveToNextLine(1));
    }

    queue.Flush();

    var ch = ReadChar();
    if (ch == '1')
    {
        InteractiveDemo.Tests.Cursor.Run(stdout);
    }

    if (ch == '2')
    {
        InteractiveDemo.Tests.Color.Run(stdout);
    }
    
    if(ch == '3')
    {
        InteractiveDemo.Tests.Attribute.Run(stdout);
    }

    if (ch == '4')
    {
        InteractiveDemo.Tests.Event.Run(stdout);
    }

    else if (ch == 'q')
    {
        break;
    }
}

stdout
    .Execute(Show)
    .Execute(LeaveAlternateScreen);

Terminal.DisableRawMode();


static char ReadChar()
{
    while (true)
    {
        var read = EventStream.Default.Read();
        if (read is Event.KeyEvent { Event.Code: KeyCode.CharKeyCode ch })
        {
            return ch.Character;
        }
    }
}
