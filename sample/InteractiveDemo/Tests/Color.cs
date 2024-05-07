using Tutu.Extensions;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Style;

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
                    SetBackgroundColor(Tutu.Style.Types.Color.Black),
                    Print(SetWidth($"{color.Name} ████████████ ", 40)),
                    SetBackgroundColor(Tutu.Style.Types.Color.White),
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
                    SetForegroundColor(Tutu.Style.Types.Color.Black),
                    Print(SetWidth($"{color.Name} ▒▒▒▒▒▒▒▒▒▒▒▒ ", 40)),
                    SetForegroundColor(Tutu.Style.Types.Color.White),
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
            (col, row) => Tutu.Style.Types.Color.AnsiValue((byte)(row * 16 + col)));
    }

    private static void TestRgbRedValues(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Rgb Red values",
            (col, row) => Tutu.Style.Types.Color.Rgb((byte)(col * 16), 0, 0));
    }

    private static void TestRgbGreenValues(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Rgb Green values",
            (col, row) => Tutu.Style.Types.Color.Rgb(0, (byte)(col * 16), 0));
    }

    private static void TestRgbBlueValues(TextWriter writer)
    {
        TestColorValuesMatrix16x16(writer,
            "Color::Rgb Blue values",
            (col, row) => Tutu.Style.Types.Color.Rgb(0, 0, (byte)(col * 16)));
    }

    private static void TestColorValuesMatrix16x16(TextWriter writer,
        string title,
        Func<ushort, ushort, Tutu.Style.Types.Color> color)
    {
        writer.Execute(Print(title));

        for (var idx = 0; idx < 16; idx++)
        {
            writer.Enqueue(
                    MoveTo(1, (idx + 4)),
                    Print(SetWidth($"{idx}", 2)))
                .Flush();

            writer.Enqueue(
                    MoveTo((idx * 3 + 3), 3),
                    Print(SetWidth($"{idx}", 3)))
                .Flush();
        }

        for (var row = 0; row <= 15; row++)
        {
            writer.Execute(MoveTo(4, (row + 4)));

            for (var col = 0; col <= 15; col++)
            {
                writer.Enqueue(
                        SetForegroundColor(color((ushort)col, (ushort)row)),
                        Print("███"))
                    .Flush();
            }

            writer.Enqueue(
                    SetForegroundColor(Tutu.Style.Types.Color.White),
                    Print(SetWidth($"{row * 16}", 3) + " .. = "),
                    Print(SetWidth($"{row * 16 + 15}", 3)))
                .Flush();
        }

        writer.Flush();
    }



    private static List<Tutu.Style.Types.Color> Colors =>
    [
        Tutu.Style.Types.Color.Black,
        Tutu.Style.Types.Color.DarkGrey,
        Tutu.Style.Types.Color.Grey,
        Tutu.Style.Types.Color.White,
        Tutu.Style.Types.Color.DarkRed,
        Tutu.Style.Types.Color.Red,
        Tutu.Style.Types.Color.DarkGreen,
        Tutu.Style.Types.Color.Green,
        Tutu.Style.Types.Color.DarkYellow,
        Tutu.Style.Types.Color.Yellow,
        Tutu.Style.Types.Color.DarkBlue,
        Tutu.Style.Types.Color.Blue,
        Tutu.Style.Types.Color.DarkMagenta,
        Tutu.Style.Types.Color.Magenta,
        Tutu.Style.Types.Color.DarkCyan,
        Tutu.Style.Types.Color.Cyan,
        Tutu.Style.Types.Color.AnsiValue(15),
        Tutu.Style.Types.Color.Rgb(255, 0, 0),
        Tutu.Style.Types.Color.Rgb(0, 255, 0),
        Tutu.Style.Types.Color.Rgb(0, 0, 255)
    ];
}
