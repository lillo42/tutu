using System.Runtime.InteropServices;
using Tutu.Unix;
using Tutu.Windows;

namespace Tutu.Tty;

/// <summary>
/// The static instance of <see cref="ITty"/>. 
/// </summary>
/// <remarks>
/// It's a wrapper for <see cref="ITty"/>.
/// </remarks>
public static class SystemTty
{
    /// <summary>
    /// The current <see cref="ITty"/> instance.
    /// </summary>
    public static ITty Instance { get; }

    /// <summary>
    /// Initialize a new instance of <see cref="SystemTty"/>.
    /// </summary>
    static SystemTty()
    {
        Instance = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsTty() : new UnixTty();
    }

    /// <inheritdoc cref="ITty.IsTty"/>
    public static bool IsTty => Instance.IsTty;
}
