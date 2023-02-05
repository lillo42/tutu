using static Erised.Commands.Cursor;
using static Erised.Commands.Style;

namespace InteractiveDemo.Tests;

public class Color : AbstractTest
{
    public static void Run(TextWriter writer)
    {
        Execute(writer,
            TestSetForegroundColor,
            TestSetBackgroundColor,
            TestColorAnsiValue,
            TestRgbRedValues,
            TestRgbGreenValues,
            TestRgbBlueValues);
    }

    private static void TestSetForegroundColor(TextWriter writer)
    {
        writer.Enqueue(
                Print("Foreground colors on the black & white background"),
                MoveToNextLine(2))
            .Flush();

        foreach (var color in Colors)
        {
            writer.Enqueue(
                    SetForegroundColor(color),
                    SetBackgroundColor(Erised.Style.Types.Color.Black),
                    Print(SetWidth($"{color.Name} ████████████ ", 40)),
                    SetBackgroundColor(Erised.Style.Types.Color.White),
                    Print(SetWidth($"{color.Name} ████████████ ", 40)),
                    MoveToNextLine(1))
                .Flush();
        }

        writer.Flush();
    }

    private static void TestSetBackgroundColor(TextWriter writer)
    {
        writer.Enqueue(
                Print("Background colors with black & white foreground"),
                MoveToNextLine(2))
            .Flush();

        foreach (var color in Colors)
        {
            writer.Enqueue(
                    SetBackgroundColor(color),
                    SetForegroundColor(Erised.Style.Types.Color.Black),
                    Print(SetWidth($"{color.Name} ▒▒▒▒▒▒▒▒▒▒▒▒ ", 40)),
                    SetForegroundColor(Erised.Style.Types.Color.White),
                    Print(SetWidth($"{color.Name} ▒▒▒▒▒▒▒▒▒▒▒▒ ", 40)),
                    MoveToNextLine(1))
                .Flush();
        }

        writer.Flush();
    }

    private static void TestColorAnsiValue(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Ansi value",
            (col, row) => Erised.Style.Types.Color.AnsiValue((byte)(row * 16 + col)));
    }

    private static void TestRgbRedValues(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Rgb Red values",
            (col, row) => Erised.Style.Types.Color.Rgb((byte)(col * 16), 0, 0));
    }

    private static void TestRgbGreenValues(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Rgb Green values",
            (col, row) => Erised.Style.Types.Color.Rgb(0, (byte)(col * 16), 0));
    }

    private static void TestRgbBlueValues(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Rgb Blue values",
            (col, row) => Erised.Style.Types.Color.Rgb(0, 0, (byte)(col * 16)));
    }

    private static void TestColorValuesMatrix16x16(TextWriter writer,
        string title,
        Func<ushort, ushort, Erised.Style.Types.Color> color)
    {
        writer.Execute(Print(title));

        for (var idx = 0; idx < 16; idx++)
        {
            writer.Enqueue(
                    MoveTo(1, (ushort)(idx + 4)),
                    Print(SetWidth($"{idx}", 2)))
                .Flush();

            writer.Enqueue(
                    MoveTo((ushort)(idx * 3 + 3), 3),
                    Print(SetWidth($"{idx}", 3)))
                .Flush();
        }

        for (var row = 0; row <= 15; row++)
        {
            writer.Execute(MoveTo(4, (ushort)(row + 4)));

            for (var col = 0; col <= 15; col++)
            {
                writer.Enqueue(
                        SetForegroundColor(color((ushort)col, (ushort)row)),
                        Print("███"))
                    .Flush();
            }

            writer.Enqueue(
                    SetForegroundColor(Erised.Style.Types.Color.White),
                    Print(SetWidth($"{row * 16}", 3) + " .. = "),
                    Print(SetWidth($"{row * 16 + 15}", 3)))
                .Flush();
        }

        writer.Flush();
    }

   

    private static List<Erised.Style.Types.Color> Colors => new()
    {
        Erised.Style.Types.Color.Black,
        Erised.Style.Types.Color.DarkGrey,
        Erised.Style.Types.Color.Grey,
        Erised.Style.Types.Color.White,
        Erised.Style.Types.Color.DarkRed,
        Erised.Style.Types.Color.Red,
        Erised.Style.Types.Color.DarkGreen,
        Erised.Style.Types.Color.Green,
        Erised.Style.Types.Color.DarkYellow,
        Erised.Style.Types.Color.Yellow,
        Erised.Style.Types.Color.DarkBlue,
        Erised.Style.Types.Color.Blue,
        Erised.Style.Types.Color.DarkMagenta,
        Erised.Style.Types.Color.Magenta,
        Erised.Style.Types.Color.DarkCyan,
        Erised.Style.Types.Color.Cyan,
        Erised.Style.Types.Color.AnsiValue(15),
        Erised.Style.Types.Color.Rgb(255, 0, 0),
        Erised.Style.Types.Color.Rgb(0, 255, 0),
        Erised.Style.Types.Color.Rgb(0, 0, 255),
    };
}