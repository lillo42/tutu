using System.Runtime.InteropServices;
using static Erised.Windows.Interop.Kernel32;

namespace Erised;

internal static partial class Windows
{
    private static ulong _savedCursorPosition = ulong.MaxValue;

    /// <summary>
    /// The 'y' position of the cursor is not relative to the window but absolute to screen buffer.
    /// We can calculate the relative cursor position by subtracting the top position of the terminal window from the y position.
    /// This results in an 1-based coord zo subtract 1 to make cursor position 0-based.
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    private static short ParseRelativeY(short y)
    {
        var windows = ScreenBuffer.Current.Info;

        var windowSize = windows.TerminalWindow;
        var screenSize = windows.TerminalSize;

        if (y <= screenSize.Height)
        {
            return y;
        }

        return (short)(y - windowSize.Top);
    }

    /// <summary>
    /// The cursor position (column, row).
    /// </summary>
    public static (ushort Column, ushort Row) Position
    {
        get
        {
            var pos = Output.Info.CursorPos;
            return ((ushort)pos.X, (ushort)ParseRelativeY(pos.Y));
        }
    }

    public static void Show() => SetVisibility(true);

    public static void Hide() => SetVisibility(false);

    private static void SetVisibility(bool visible)
    {
        var cursorInfo = new CONSOLE_CURSOR_INFO
        {
            dwSize = 100,
            bVisible = visible ? 1 : 0
        };

        var handle = Handle.CurrentOutHandle();
        if (!SetConsoleCursorInfo(handle, ref cursorInfo))
        {
            throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
        }
    }

    public static void MoveTo(ushort column, ushort row)
    {
        var position = new COORD { X = (short)column, Y = (short)row };
        if (!SetConsoleCursorPosition(Output.Handle, position))
        {
            throw GetExceptionForWin32Error(Marshal.GetLastPInvokeError());
        }
    }

    public static void MoveToNextLine(ushort count)
    {
        var (_, row) = Position;
        MoveTo(0, (ushort)(row + count));
    }

    public static void MoveToPreviousLine(ushort count)
    {
        var (_, row) = Position;
        MoveTo(0, (ushort)(row - count));
    }

    public static void MoveToColumn(ushort newColumn)
    {
        var (_, row) = Position;
        MoveTo(newColumn, row);
    }

    public static void MoveToRow(ushort newRow)
    {
        var (column, _) = Position;
        MoveTo(column, newRow);
    }

    public static void MoveUp(ushort count)
    {
        var (column, row) = Position;
        MoveTo(column, (ushort)(row - count));
    }

    public static void MoveRight(ushort count)
    {
        var (column, row) = Position;
        MoveTo((ushort)(column + count), row);
    }

    public static void MoveDown(ushort count)
    {
        var (column, row) = Position;
        MoveTo(column, (ushort)(row + count));
    }

    public static void MoveLeft(ushort count)
    {
        var (column, row) = Position;
        MoveTo((ushort)(column - count), row);
    }

    public static void SavePosition()
    {
        var (column, row) = Output.Info.CursorPos;

        var position = Convert.ToUInt64((Convert.ToUInt32((ushort)column) << 16) | Convert.ToUInt32((ushort)row));
        Interlocked.Exchange(ref _savedCursorPosition, position);
    }

    public static void RestorePosition()
    {
        var position = Interlocked.Read(ref _savedCursorPosition);
        if (position == ulong.MaxValue)
        {
            return;
        }

        var column = (ushort)(position >> 16);
        var row = (ushort)position;

        MoveTo(column, row);
    }

    private static ScreenBuffer Output => new(Handle.Create(HandleType.CurrentOutputHandle));
}