using Tutu.Extensions;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Style;

namespace InteractiveDemo.Tests;

public class Attribute : AbstractTest
{
    private static (Tutu.Style.Types.Attribute, Tutu.Style.Types.Attribute)[] Attibuttes =
    {
        (Tutu.Style.Types.Attribute.Bold, Tutu.Style.Types.Attribute.NormalIntensity),
        (Tutu.Style.Types.Attribute.Italic, Tutu.Style.Types.Attribute.NoItalic),
        (Tutu.Style.Types.Attribute.Underlined, Tutu.Style.Types.Attribute.NoUnderline),

        (Tutu.Style.Types.Attribute.DoubleUnderlined, Tutu.Style.Types.Attribute.NoUnderline),
        (Tutu.Style.Types.Attribute.Undercurled, Tutu.Style.Types.Attribute.NoUnderline),
        (Tutu.Style.Types.Attribute.Underdotted, Tutu.Style.Types.Attribute.NoUnderline),
        (Tutu.Style.Types.Attribute.Underdashed, Tutu.Style.Types.Attribute.NoUnderline),

        (Tutu.Style.Types.Attribute.Reverse, Tutu.Style.Types.Attribute.NoReverse),
        (Tutu.Style.Types.Attribute.CrossedOut, Tutu.Style.Types.Attribute.NotCrossedOut),
        (Tutu.Style.Types.Attribute.SlowBlink, Tutu.Style.Types.Attribute.NoBlink),
    };

    private static void TestSetDisplayAttribute(TextWriter writer)
    {
        writer.Enqueue(
                Print("Display attributes"),
                MoveToNextLine(2))
            .Flush();

        foreach (var (on, off) in Attibuttes)
        {
            writer.Enqueue(
                    SetAttribute(on),
                    Print(SetWidth($"{on.Name} ", 35)),
                    SetAttribute(off),
                    Print(SetWidth($"{off.Name}", 35)),
                    Reset,
                    MoveToNextLine(1))
                .Flush();
        }

        writer.Flush();
    }

    public static void Run(TextWriter writer)
    {
        Execute(writer, TestSetDisplayAttribute);
    }
}