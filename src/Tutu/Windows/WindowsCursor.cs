using System.Runtime.InteropServices;
using Tutu.Cursor;
using Tutu.Windows.Interop.Kernel32;
using static Tutu.Windows.Interop.Kernel32.Kernel32;

namespace Tutu.Windows;

/// <summary>
/// Windows implementation of <see cref="ICursor"/>.
/// </summary>
public class WindowsCursor : ICursor
{
    private static ulong _savedCursorPosition = ulong.MaxValue;

    /// <inheritdoc cref="ICursor.Position"/>
    public CursorPosition Position => GetPosition(ScreenBuffer.CurrentOutput);

    private static CursorPosition GetPosition(ScreenBuffer buffer)
    {
        var pos = buffer.Info.CursorPosition;
        return new(pos.X, ParseRelativeY(buffer, pos.Y));
    }

    /// <summary>
    /// The 'y' position of the cursor is not relative to the window but absolute to screen buffer.
    /// We can calculate the relative cursor position by subtracting the top position of the terminal window from the y position.
    /// This results in an 1-based coord zo subtract 1 to make cursor position 0-based.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static int ParseRelativeY(ScreenBuffer buffer, short y)
    {
        var windows = buffer.Info;
        var windowsSize = windows.TerminalWindow;
        var screenSize = windows.TerminalSize;
        if (y <= screenSize.Height)
        {
            return y;
        }

        return y - windowsSize.Top;
    }

    /// <summary>
    /// Show the cursor.
    /// </summary>
    public static void Show() => SetVisibility(true);

    /// <summary>
    /// Hide the cursor.
    /// </summary>
    public static void Hide() => SetVisibility(false);

    private static void SetVisibility(bool visible)
    {
        var cursorInfo = new CONSOLE_CURSOR_INFO
        {
            dwSize = 100,
            bVisible = visible ? 1 : 0
        };

        var handle = Handle.Create(HandleType.CurrentOutput);
        if (!SetConsoleCursorInfo(handle, ref cursorInfo))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }

    private static void MoveTo(ScreenBuffer buffer, ushort column, ushort row)
    {
        var position = new COORD { X = (short)column, Y = (short)row };
        if (!SetConsoleCursorPosition(buffer.Handle, position))
        {
            Marshal.ThrowExceptionForHR(Marshal.GetLastPInvokeError());
        }
    }

    internal static void MoveTo(ushort column, ushort row)
        => MoveTo(ScreenBuffer.CurrentOutput, column, row);

    internal static void MoveToNextLine(ushort count)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (_, row) = GetPosition(buffer);
        MoveTo(buffer, 0, (ushort)(row + count));
    }

    internal static void MoveToPreviousLine(ushort count)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (_, row) = GetPosition(buffer);
        MoveTo(buffer, 0, (ushort)(row - count));
    }

    internal static void MoveToColumn(ushort newColumn)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (_, row) = GetPosition(buffer);
        MoveTo(buffer, newColumn, (ushort)row);
    }

    internal static void MoveToRow(ushort newRow)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (column, _) = GetPosition(buffer);
        MoveTo(buffer, (ushort)column, newRow);
    }

    internal static void MoveUp(ushort count)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (column, row) = GetPosition(buffer);
        MoveTo(buffer, (ushort)column, (ushort)(row - count));
    }

    internal static void MoveRight(ushort count)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (column, row) = GetPosition(buffer);
        MoveTo((ushort)(column + count), (ushort)row);
    }

    internal static void MoveDown(ushort count)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (column, row) = GetPosition(buffer);
        MoveTo(buffer, (ushort)column, (ushort)(row + count));
    }

    internal static void MoveLeft(ushort count)
    {
        var buffer = ScreenBuffer.CurrentOutput;
        var (column, row) = GetPosition(buffer);
        MoveTo(buffer, (ushort)(column - count), (ushort)row);
    }

    internal static void SavePosition()
    {
        var (column, row) = ScreenBuffer.CurrentOutput.Info.CursorPosition;

        var position = Convert.ToUInt64((Convert.ToUInt32((ushort)column) << 16) | Convert.ToUInt32((ushort)row));
        Interlocked.Exchange(ref _savedCursorPosition, position);
    }

    internal static void RestorePosition()
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
}
