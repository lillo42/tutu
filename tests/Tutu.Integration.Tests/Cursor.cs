using FluentAssertions;
using Tutu.Cursor;
using Tutu.Extensions;
using static Tutu.Commands.Cursor;

namespace Tutu.Integration.Tests;

public class Cursor
{
    private readonly ICursor _cursor;

    public Cursor()
    {
        _cursor = Tutu.Cursor.Cursor.Instance;
    }

    [Fact]
    public void MoveCursorTo()
    {
        var (x, y) = _cursor.Position;

        Console.Out.Execute(MoveTo(x + 1, y + 1));
        _cursor.Position.Should().Be(new CursorPosition(x + 1, y + 1));

        Console.Out.Execute(MoveTo(x, y));
        _cursor.Position.Should().Be(new CursorPosition(x, y));
    }

    [Fact]
    public void MoveCursorRight()
    {
        var (x, y) = _cursor.Position;
        Console.Out.Execute(MoveRight(1));
        _cursor.Position.Should().Be(new CursorPosition(x + 1, y));
    }

    [Fact]
    public void MoveCursorLeft()
    {
        Console.Out.Execute(MoveTo(2, 0), MoveLeft(2));
        _cursor.Position.Should().Be(new CursorPosition(0, 0));
    }

    [Fact]
    public void MoveCursorUp()
    {
        Console.Out.Execute(MoveTo(0, 2), MoveUp(2));
        _cursor.Position.Should().Be(new CursorPosition(0, 0));
    }

    [Fact]
    public void MoveCursorDown()
    {
        Console.Out.Execute(MoveDown(2));
        _cursor.Position.Should().Be(new CursorPosition(0, 2));
    }

    [Fact]
    public void SaveRestorePosition()
    {
        var (x, y) = _cursor.Position;

        Console.Out.Execute(SavePosition, MoveTo(x + 1, y + 1), RestorePosition);
        
        _cursor.Position.Should().Be(new CursorPosition(x, y));
    }
}