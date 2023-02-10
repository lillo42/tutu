using Tmds.Linux;

namespace Erised;

internal static partial class Unix 
{
    public static class Terminal
    {
        private static readonly Mutex<termios?> TerminalModePriorRawMode = new(null);
        
        public static bool IsRawModeEnabled
        {
            get
            {
                using var value = TerminalModePriorRawMode.Lock();
                return value.Value != null;
            }
        }

        public static void EnableRawMode()
        {
            using var originalMode = TerminalModePriorRawMode.Lock();
            if (originalMode.Value != null)
            {
                return;
            }
        }
        
        public static void DisableRawMode()
        {
            using var originalMode = TerminalModePriorRawMode.Lock();
            if (originalMode.Value == null)
            {
                return;
            }
        }
    }
}