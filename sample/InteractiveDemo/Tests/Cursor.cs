using Tutu;
using Tutu.Cursor;
using Tutu.Extensions;
using Tutu.Style.Extensions;
using static Tutu.Commands.Cursor;
using static Tutu.Commands.Style;

namespace InteractiveDemo.Tests;

public class Cursor : AbstractTest
{
    public static void Run(TextWriter writer)
    {
        Execute(writer,
            TestHideCursor,
            TestShowCursor,

            // Blinking
            TestBlinkingBar,
            TestBlinkingBlock,
            TestBlinkingUnderScore,

            // Cursor movement
            TestMoveCursorLeft,
            TestMoveCursorRight,
            TestMoveCursorUp,
            TestMoveCursorDown,
            TestMoveCursorTo,
            TestMoveCursorToNextLine,
            TestMoveCursorToPreviousLine,
            TestMoveCursorToColumn,

            // Save and restore
            TestSaveAndRestoreCursorPosition
        );
    }

    private static void TestHideCursor(TextWriter writer) => writer.Execute(Print("Hide Cursor"), Hide);
    private static void TestShowCursor(TextWriter writer) => writer.Execute(Print("Show Cursor"), Show);


    private static void TestBlinkingBar(TextWriter writer)
        => writer.Execute(Print("Blinking Bar:"), SetCursorStyle(CursorStyle.BlinkingBar));

    private static void TestBlinkingBlock(TextWriter writer)
        => writer.Execute(Print("Blinking Block:"), SetCursorStyle(CursorStyle.BlinkingBlock));

    private static void TestBlinkingUnderScore(TextWriter writer)
        => writer.Execute(Print("Blinking UnderSore:"), SetCursorStyle(CursorStyle.BlinkingUnderScore));

    private static void TestSaveAndRestoreCursorPosition(TextWriter writer)
    {
        writer
            .Enqueue(
                MoveTo(0, 0),
                Print("Save position, print character elsewhere, after three seconds restore to old position."),
                MoveToNextLine(2),
                Print("Save ->[ ]<- Position"),
                MoveTo(8, 2),
                SavePosition,
                MoveTo(10, 10),
                Print("Move To ->[√]<- Position"))
            .Flush();

        Thread.Sleep(3000);

        writer
            .Execute(
                RestorePosition,
                Print("√"));
    }


    private static void TestMoveCursorUp(TextWriter writer)
        => DrawCursorBox(writer, "Move Up(2)", (_, _) => MoveUp(2));

    private static void TestMoveCursorDown(TextWriter writer)
        => DrawCursorBox(writer, "Move Down(2)", (_, _) => MoveDown(2));

    private static void TestMoveCursorLeft(TextWriter writer)
        => DrawCursorBox(writer, "Move Left(2)", (_, _) => MoveLeft(2));

    private static void TestMoveCursorRight(TextWriter writer)
        => DrawCursorBox(writer, "Move Right(2)", (_, _) => MoveRight(2));

    private static void TestMoveCursorToPreviousLine(TextWriter writer)
        => DrawCursorBox(writer, "Move To Previous Line(2)", (_, _) => MoveToPreviousLine(2));

    private static void TestMoveCursorToNextLine(TextWriter writer)
        => DrawCursorBox(writer, "Move To Next Line(2)", (_, _) => MoveToNextLine(2));

    private static void TestMoveCursorToColumn(TextWriter writer)
        => DrawCursorBox(writer, "Move To Column(1)",
            (centerX, _) => MoveToColumn((ushort)(centerX + 1)));

    private static void TestMoveCursorTo(TextWriter writer)
        => DrawCursorBox(writer, "Move To (x: 1, y :1) from center",
            (centerX, centerY) => MoveTo((ushort)(centerX + 1), (ushort)(centerY + 1)));

    private static void DrawCursorBox(TextWriter writer, string description, Func<ushort, ushort, ICommand> command)
    {
        writer
            .Execute(
                Hide,
                MoveTo(0, 0),
                SetForegroundColor(Tutu.Style.Types.Color.Red),
                Print(
                    $"Red box is the center. After the action: '{description}' '√' is drawn to reflect the action from the center."));

        var startY = 2;
        var width = 21;
        var height = 11 + startY;
        var centerX = width / 2;
        var centerY = (height + startY) / 2;

        for (var row = startY; row <= startY + 10; row++)
        {
            for (var column = 0; column <= width; column++)
            {
                if ((row == startY || row == height - 1) || (column == 0 || column == width))
                {
                    writer.Enqueue(MoveTo((ushort)column, (ushort)row),
                        PrintStyledContent("▓".Red()))
                        .Flush();
                }
                else
                {
                    writer.Enqueue(MoveTo((ushort)column, (ushort)row),
                            PrintStyledContent("_".Red().OnWhite()))
                        .Flush();
                }
            }
        }

        writer
            .Enqueue(
                MoveTo((ushort)centerX, (ushort)centerY),
                PrintStyledContent("▀".Red().OnWhite()),
                MoveTo((ushort)centerX, (ushort)centerY)
            )
            .Flush();

        writer
            .Enqueue(
                command((ushort)centerX, (ushort)centerY),
                PrintStyledContent("√".Magenta().OnWhite()))
            .Flush();
        writer.Flush();
    }
}