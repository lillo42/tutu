namespace Erised;

public static class Tty
{
    public static bool IsTty => Windows.IsTty;
}