using System.Runtime.InteropServices;

namespace Erised.Terminal;

public static class Terminal
{
    public static bool IsRawModeEnable
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Windows.Terminal.IsRawModeEnabled;
            }

            return Unix.Terminal.IsRawModeEnabled;
        }
    }

    public static void EnableRawMode()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Windows.Terminal.EnableRawMode();
        }
        else
        {
            Unix.Terminal.EnableRawMode();
        }
    }

    public static void DisableRawMode() => Windows.Terminal.DisableRawMode();

    public static (ushort Width, ushort Height) Size
    {
        get => Windows.Terminal.Size;
        set => Windows.Terminal.Size = value;
    }

    public static bool SupportsKeyboardEnhancement => false;

    public static void Clear(ClearType type) => Windows.Terminal.Clear(type);

    public static void ScrollUp(ushort lines) => Windows.Terminal.ScrollUp(lines);
    public static void ScrollDown(ushort lines) => Windows.Terminal.ScrollDown(lines);

    public static void SetTitle(string title) => Windows.Terminal.SetWindowTitle(title);
}