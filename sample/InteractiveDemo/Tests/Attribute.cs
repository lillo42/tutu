using static Erised.Commands.Cursor;
using static Erised.Commands.Style;

namespace InteractiveDemo.Tests;

public class Attribute : AbstractTest
{
    private static (Erised.Style.Types.Attribute, Erised.Style.Types.Attribute)[] Attibuttes =
    {
        (Erised.Style.Types.Attribute.Bold, Erised.Style.Types.Attribute.NormalIntensity),
        (Erised.Style.Types.Attribute.Italic, Erised.Style.Types.Attribute.NoItalic),
        (Erised.Style.Types.Attribute.Underlined, Erised.Style.Types.Attribute.NoUnderline),

        (Erised.Style.Types.Attribute.DoubleUnderlined, Erised.Style.Types.Attribute.NoUnderline),
        (Erised.Style.Types.Attribute.Undercurled, Erised.Style.Types.Attribute.NoUnderline),
        (Erised.Style.Types.Attribute.Underdotted, Erised.Style.Types.Attribute.NoUnderline),
        (Erised.Style.Types.Attribute.Underdashed, Erised.Style.Types.Attribute.NoUnderline),

        (Erised.Style.Types.Attribute.Reverse, Erised.Style.Types.Attribute.NoReverse),
        (Erised.Style.Types.Attribute.CrossedOut, Erised.Style.Types.Attribute.NotCrossedOut),
        (Erised.Style.Types.Attribute.SlowBlink, Erised.Style.Types.Attribute.NoBlink),
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