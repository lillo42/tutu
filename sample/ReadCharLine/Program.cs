// Demonstrates how to block read characters or a full line.
// Just note that Tutu is not required to do this and can be done with `Console.ReadLine`.

using System.Text;
using Tutu.Events;

Console.WriteLine("read line:");
Console.WriteLine(ReadLine());

Console.WriteLine("read char:");
Console.WriteLine(ReadChar());

static string ReadLine()
{
    var line = new StringBuilder();

    while (true)
    {
        var read = EventReader.Read();
        if (read is Event.KeyEventEvent { Event: { Code: KeyCode.CharKeyCode ch, Kind: KeyEventKind.Release } })
        {
            line.Append(ch.Character);
        }
        else if (read is Event.KeyEventEvent { Event.Code: KeyCode.EnterKeyCode })
        {
            break;
        }
    }

    return line.ToString();
}

static char ReadChar()
{
    while (true)
    {
        var read = EventReader.Read();
        if (read is Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode ch })
        {
            return ch.Character;
        }
    }
}
