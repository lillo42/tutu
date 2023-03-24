// Demonstrates how to block read characters or a full line.
// Just note that Tutu is not required to do this and can be done with `Console.ReadLine`.

using System.Runtime.InteropServices;
using System.Text;
using NodaTime;
using Tutu.Events;

// Windows is returning Enter that have been used to start the sample
// when we use dotnet run
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && SystemEventReader.Instance.Poll(Duration.FromMilliseconds(10)))
{
    _ = SystemEventReader.Instance.Read();
}

Console.WriteLine("read line:");
Console.WriteLine(ReadLine());

Console.WriteLine("read char:");
Console.WriteLine(ReadChar());

static string ReadLine()
{
    var line = new StringBuilder();

    while (true)
    {
        var read = SystemEventReader.Instance.Read();
        if (read is Event.KeyEventEvent { Event: { Code: KeyCode.CharKeyCode ch, Kind: KeyEventKind.Press } })
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

static string ReadChar()
{
    while (true)
    {
        var read = SystemEventReader.Instance.Read();
        if (read is Event.KeyEventEvent { Event.Code: KeyCode.CharKeyCode ch })
        {
            return ch.Character;
        }
    }
}
