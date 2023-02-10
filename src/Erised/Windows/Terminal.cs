using System.Text;
using Erised.Terminal;
using static Erised.Windows.Interop.Kernel32;

namespace Erised;

internal static partial class Windows
{
    internal static partial class Terminal
    {
        private const uint EnableLineInput = 0x0002;
        private const uint EnableEchoInput = 0x0004;
        private const uint EnableProcessedInput = 0x0001;
        private const uint NotRawModeMask = EnableLineInput | EnableEchoInput | EnableProcessedInput;

        public static bool IsRawModeEnabled => (Console.Input.Mode & NotRawModeMask) == 0;

        public static void EnableRawMode()
        {
            var console = Console.Input;
            console.Mode &= ~NotRawModeMask;
        }

        public static void DisableRawMode()
        {
            var console = Console.Input;
            console.Mode |= NotRawModeMask;
        }

        public static bool SupportsKeyboardEnhancement => false;

        public static (ushort Width, ushort Height) Size
        {
            get
            {
                var size = ScreenBuffer.Current.Info.TerminalSize;
                return ((ushort)(size.Width + 1), (ushort)(size.Height + 1));
            }
            set
            {
                if (value.Width < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value.Width), value.Width, "Positive number required.");
                }

                if (value.Height < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value.Height), value.Height, "Width must be greater than 0.");
                }

                var screenBuffer = ScreenBuffer.Current;
                var console = new Console(screenBuffer.Handle);
                var csbi = screenBuffer.Info;

                var currentSize = csbi.TerminalSize;
                var windowSize = csbi.TerminalWindow;

                var newSize = new Size(currentSize.Width, currentSize.Height);
                var width = (short)value.Width;

                var resize = false;
                if (currentSize.Width < windowSize.Left + width)
                {
                    if (windowSize.Left >= short.MaxValue - width)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value.Width), $"The value must be less than the console's current maximum window size of {short.MaxValue - value.Width} in that dimension. Note that this value depends on screen resolution and the console font.");
                    }

                    newSize = newSize with { Width = (short)(windowSize.Left + width) };
                    resize = true;
                }

                var height = (short)value.Height;
                if (currentSize.Height < windowSize.Top + height)
                {
                    if (windowSize.Top >= short.MaxValue - height)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value.Height), $"The value must be less than the console's current maximum window size of {short.MaxValue - value.Height} in that dimension. Note that this value depends on screen resolution and the console font.");
                    }

                    newSize = newSize with { Height = (short)(windowSize.Top + height) };
                    resize = true;
                }

                if (resize)
                {
                    screenBuffer.SetSize((short)(newSize.Width - 1), (short)(newSize.Height - 1));
                }

                windowSize = windowSize with
                {
                    Bottom = (short)(windowSize.Top + height - 1),
                    Right = (short)(windowSize.Left + width - 1)
                };

                console.SetConsoleInfo(true, windowSize);

                if (resize)
                {
                    screenBuffer.SetSize((short)(newSize.Width - 1), (short)(newSize.Height - 1));
                }


                var bounds = console.LargestWindowsSize();

                if (width > bounds.X)
                {
                    throw new ArgumentOutOfRangeException(nameof(value.Width), width, "The new console window size would force the console buffer size to be too large.");
                }

                if (height > bounds.Y)
                {
                    throw new ArgumentOutOfRangeException(nameof(value.Height), height, "The new console window size would force the console buffer size to be too large.");
                }
            }
        }

        public static void Clear(ClearType clearType)
        {
            var screenBuffer = ScreenBuffer.Current;
            var info = screenBuffer.Info;

            var pos = info.CursorPos;
            var bufferSize = info.BufferSize;
            var currentAttribute = info.Attributes;

            switch (clearType)
            {
                case ClearType.FromCursorDown:
                    ClearAfterCursor(pos, bufferSize, currentAttribute);
                    break;
                case ClearType.FromCursorUp:
                    ClearBeforeCursor(pos, bufferSize, currentAttribute);
                    break;
                case ClearType.CurrentLine:
                    ClearCurrentLine(pos, bufferSize, currentAttribute);
                    break;
                case ClearType.UntilNewLine:
                    ClearUntilLine(pos, bufferSize, currentAttribute);
                    break;
                default:
                    ClearEntireScreen(bufferSize, currentAttribute);
                    break;
            }
        }

        public static void ScrollUp(ushort rowCount)
        {
            var screenBuffer = ScreenBuffer.Current;

            var windows = screenBuffer.Info.TerminalWindow;
            var count = (short)rowCount;
            if (windows.Top >= count)
            {
                windows = windows with
                {
                    Top = (short)(windows.Top - count),
                    Bottom = (short)(windows.Bottom - count)
                };

                Console.Output.SetConsoleInfo(true, windows);
            }
        }

        public static void ScrollDown(ushort rowCount)
        {
            var info = ScreenBuffer.Current.Info;

            var window = info.TerminalWindow;
            var bufferSize = info.BufferSize;

            var count = (short)rowCount;
            if (window.Bottom < bufferSize.Height - count)
            {
                window = window with
                {
                    Top = (short)(window.Top + count),
                    Bottom = (short)(window.Bottom + count)
                };

                Console.Output.SetConsoleInfo(true, window);
            }
        }

        public static void SetWindowTitle(string title)
            => SetConsoleTitle(Encoding.Unicode.GetString(Encoding.UTF8.GetBytes(title)));

        private static void ClearEntireScreen(Size bufferSize, ushort currentAttribute)
        {
            var cellsToWrite = (uint)bufferSize.Width * (uint)bufferSize.Height;
            var startLocation = new Coordinate(0, 0);
            Clear(startLocation, cellsToWrite, currentAttribute);
            MoveTo(0, 0);
        }

        private static void ClearAfterCursor(Coordinate location, Size bufferSize, ushort currentAttribute)
        {
            var (x, y) = location;
            if (x > bufferSize.Width)
            {
                y += 1;
                x = 0;
            }

            var startLocation = new Coordinate(x, y);
            var cellsToWrite = (uint)bufferSize.Width * (uint)bufferSize.Height;
            Clear(startLocation, cellsToWrite, currentAttribute);
        }

        private static void ClearBeforeCursor(Coordinate location, Size bufferSize, ushort currentAttribute)
        {
            var (xpos, ypos) = location;

            var startLocation = new Coordinate(0, 0);
            var cellsToWrite = (uint)bufferSize.Width * (uint)ypos + (uint)xpos + 1;
            Clear(startLocation, cellsToWrite, currentAttribute);
        }

        private static void ClearCurrentLine(Coordinate location, Size bufferSize, ushort currentAttribute)
        {
            var startLocation = location with { X = 0 };
            var cellsToWrite = (uint)bufferSize.Width;
            Clear(startLocation, cellsToWrite, currentAttribute);
            MoveTo(0, (ushort)location.Y);
        }

        private static void ClearUntilLine(Coordinate location, Size bufferSize, ushort currentAttribute)
        {
            var (x, y) = location;
            var startLocation = new Coordinate(x, y);

            var cellsToWrite = (uint)(bufferSize.Width - x);
            Clear(startLocation, cellsToWrite, currentAttribute);
            MoveTo((ushort)x, (ushort)y);
        }

        private static void Clear(Coordinate startLocation, uint cellToWrite, ushort currentAttribute)
        {
            var console = new Console(Handle.Create(HandleType.CurrentOutputHandle));
            console.FillWhitCharacter(startLocation, cellToWrite, ' ');
            console.FillWitAttribute(startLocation, cellToWrite, currentAttribute);
        }
    }
}