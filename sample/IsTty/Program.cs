using Erised;
using Erised.Terminal;

var size = Terminal.Size; 
Console.WriteLine($"({size.Width}, {size.Height})");

await Task.Delay(1_000);

Console.Out.Execute(Erised.Commands.Terminal.SetSize(10, 10));

size = Terminal.Size; 
Console.WriteLine($"({size.Width}, {size.Height})");

Console.WriteLine(Tty.IsTty ? "Is TTY" : "Is not TTY");