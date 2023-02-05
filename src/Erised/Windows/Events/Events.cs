namespace Erised;

internal static partial class Windows
{
    public static partial class Event
    {
        internal const uint EnableMouseMode = 0x0010 | 0x0080 | 0x0008;
        private static ulong _originalConsoleMode = ulong.MaxValue;

        public static void EnableMouseCapture()
        {
            var console = Console.CurrentIn;

            Interlocked.CompareExchange(ref _originalConsoleMode, console.Mode, ulong.MaxValue);

            console.Mode = EnableMouseMode;
        }

        public static void DisableMouseCapture()
        {
            var originalMode = Interlocked.Read(ref _originalConsoleMode);
            var console = Console.CurrentIn;
            console.Mode = (uint)originalMode;
        }
    }
}