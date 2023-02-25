using Tutu.Style.Types;

namespace Tutu.Windows.Style;

internal static partial class Windows
{
    public static class Style
    {
        private static long _originalConsoleColor = uint.MaxValue;

        private const ushort ForegroundBlue = 0x0001; // Text color contains blue.
        private const ushort ForegroundGreen = 0x0002; // Text color contains green.
        private const ushort ForegroundRed = 0x0004; // Text color contains red.
        private const ushort ForegroundIntensity = 0x0008; // Text color is intensified.
        private const ushort BackgroundBlue = 0x0010; // Background color contains blue.
        private const ushort BackgroundGreen = 0x0020; // Background color contains green.
        private const ushort BackgroundRed = 0x0040; // Background color contains red.
        private const ushort BackgroundIntensity = 0x0080; // Background color is intensified.
        private const ushort CommonLvbLeadingByte = 0x0100; // Leading byte.
        private const ushort CommonLvbTrailingByte = 0x0200; // Trailing byte.
        private const ushort CommonLvbGridHorizontal = 0x0400; // Top horizontal.
        private const ushort CommonLvbGridLvertical = 0x0800; // Left vertical.
        private const ushort CommonLvbGridRvertical = 0x1000; // Right vertical.
        private const ushort CommonLvbReverseVideo = 0x4000; // Reverse foreground and background attribute.
        private const ushort CommonLvbUnderscore = 0x8000; // Underscore

        // /
        public static void SetForegroundColor(Color fgColor)
        {
            InitConsoleColor();

            var colorValue = From(Colored.ForegroundColor(fgColor));

            var buffer = Tutu.Windows.Windows.ScreenBuffer.Current;
            var info = buffer.Info;

            var attrs = info.Attributes;
            var bgColor = attrs | 0x0070;
            var color = (ushort)(colorValue | bgColor);
            if ((attrs & BackgroundIntensity) != 0)
            {
                color |= BackgroundIntensity;
            }

            new Tutu.Windows.Windows.Console(buffer.Handle).SetTextAttribute(color);
        }

        public static void SetBackgroundColor(Color bgColor)
        {
            InitConsoleColor();
            var colorValue = From(Colored.BackgroundColor(bgColor));
            var buffer = Tutu.Windows.Windows.ScreenBuffer.Current;
            var info = buffer.Info;

            var attrs = info.Attributes;
            var fgColor = attrs | 0x0070;
            var color = (ushort)(colorValue | fgColor);
            if ((attrs & ForegroundIntensity) != 0)
            {
                color |= BackgroundIntensity;
            }

            new Tutu.Windows.Windows.Console(buffer.Handle).SetTextAttribute(color);
        }

        internal static ushort From(Colored colored)
        {
            var color = colored.Color;
            if (colored.IsForegroundColor)
            {
                if (color == Color.Black)
                {
                    return 0;
                }

                if (color == Color.DarkGrey)
                {
                    return ForegroundIntensity;
                }

                if (color == Color.Red)
                {
                    return ForegroundIntensity | ForegroundRed;
                }

                if (color == Color.DarkRed)
                {
                    return ForegroundRed;
                }

                if (color == Color.Green)
                {
                    return ForegroundIntensity | ForegroundGreen;
                }

                if (color == Color.DarkGreen)
                {
                    return ForegroundGreen;
                }

                if (color == Color.Yellow)
                {
                    return ForegroundIntensity | ForegroundRed | ForegroundGreen;
                }

                if (color == Color.DarkYellow)
                {
                    return ForegroundRed | ForegroundGreen;
                }

                if (color == Color.Blue)
                {
                    return ForegroundIntensity | ForegroundBlue;
                }

                if (color == Color.DarkBlue)
                {
                    return ForegroundBlue;
                }

                if (color == Color.Magenta)
                {
                    return ForegroundIntensity | ForegroundRed | ForegroundBlue;
                }

                if (color == Color.DarkMagenta)
                {
                    return ForegroundRed | ForegroundBlue;
                }

                if (color == Color.Cyan)
                {
                    return ForegroundIntensity | ForegroundGreen | ForegroundBlue;
                }

                if (color == Color.DarkCyan)
                {
                    return ForegroundGreen | ForegroundBlue;
                }

                if (color == Color.White)
                {
                    return ForegroundIntensity | ForegroundRed | ForegroundGreen | ForegroundBlue;
                }

                if (color == Color.Grey)
                {
                    return ForegroundRed | ForegroundGreen | ForegroundBlue;
                }

                if (color == Color.Reset)
                {
                    var originalColor =(ushort) Interlocked.Read(ref _originalConsoleColor);
                    const ushort removeBackgroundMask =
                        BackgroundIntensity | BackgroundRed | BackgroundGreen | BackgroundBlue;

                    return (ushort)(originalColor | ~removeBackgroundMask);
                }
            }

            if (colored.IsBackgroundColor)
            {
                if (color == Color.Black)
                {
                    return 0;
                }

                if (color == Color.DarkGrey)
                {
                    return BackgroundIntensity;
                }

                if (color == Color.Red)
                {
                    return BackgroundIntensity | BackgroundRed;
                }

                if (color == Color.DarkRed)
                {
                    return BackgroundRed;
                }

                if (color == Color.Green)
                {
                    return BackgroundIntensity | BackgroundGreen;
                }

                if (color == Color.DarkGreen)
                {
                    return BackgroundGreen;
                }

                if (color == Color.Yellow)
                {
                    return BackgroundIntensity | BackgroundRed | BackgroundGreen;
                }

                if (color == Color.DarkYellow)
                {
                    return BackgroundRed | BackgroundGreen;
                }

                if (color == Color.Blue)
                {
                    return BackgroundIntensity | BackgroundBlue;
                }

                if (color == Color.DarkBlue)
                {
                    return BackgroundBlue;
                }

                if (color == Color.Magenta)
                {
                    return BackgroundIntensity | BackgroundRed | BackgroundBlue;
                }

                if (color == Color.DarkMagenta)
                {
                    return BackgroundRed | BackgroundBlue;
                }

                if (color == Color.Cyan)
                {
                    return BackgroundIntensity | BackgroundGreen | BackgroundBlue;
                }

                if (color == Color.DarkCyan)
                {
                    return BackgroundGreen | BackgroundBlue;
                }

                if (color == Color.White)
                {
                    return BackgroundIntensity | BackgroundRed | BackgroundGreen | BackgroundBlue;
                }

                if (color == Color.Grey)
                {
                    return BackgroundRed | BackgroundGreen | BackgroundBlue;
                }

                if (color == Color.Reset)
                {
                    var originalColor = Interlocked.Read(ref _originalConsoleColor);
                    const ushort removeForegroundMask =
                        ForegroundIntensity | ForegroundRed | ForegroundGreen | ForegroundBlue;

                    return (ushort)((ushort)originalColor | ~removeForegroundMask);
                }
            }

            return 0;
        }

        private static void InitConsoleColor()
        {
            var originalColor = Interlocked.Read(ref _originalConsoleColor);
            if (originalColor == uint.MaxValue)
            {
                var screenBuffer = Tutu.Windows.Windows.ScreenBuffer.Current;
                var attributes = screenBuffer.Info.Attributes;
                Interlocked.Exchange(ref _originalConsoleColor, attributes);
            }
        }

        public static void Reset()
        {
            var originalColor = Interlocked.Read(ref _originalConsoleColor);
            if (originalColor <= ushort.MaxValue)
            {
                var console = Tutu.Windows.Windows.Console.CurrentOut;
                console.SetTextAttribute((ushort)originalColor);
            }
        }
    }
}