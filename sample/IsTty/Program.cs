using Tutu.Extensions;
using Tutu.Terminal;
using Tutu.Tty;

var size = SystemTerminal.Size;
Console.WriteLine($"({size.Width}, {size.Height})");

await Task.Delay(1_000);

Console.Out.Execute(Tutu.Commands.Terminal.SetSize(10, 10));

size = SystemTerminal.Size;
Console.WriteLine($"({size.Width}, {size.Height})");

Console.WriteLine(SystemTty.IsTty ? "Is TTY" : "Is not TTY");
