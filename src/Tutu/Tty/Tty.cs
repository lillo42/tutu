using System.Runtime.InteropServices;
using Tutu.Windows2;

namespace Tutu.Tty;

/// <summary>
/// The static instance of <see cref="ITty"/>. 
/// </summary>
/// <remarks>
/// It's a wrapper for <see cref="ITty"/>.
/// </remarks>
public static class Tty
{
    /// <summary>
    /// The current <see cref="ITty"/> instance.
    /// </summary>
    public static ITty Instance { get; }

    /// <summary>
    /// Initialize a new instance of <see cref="Tty"/>.
    /// </summary>
    static Tty()
    {
        Instance = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsTty() : null!;
    }

    /// <inheritdoc cref="ITty.IsTty"/>
    public static bool IsTty => Instance.IsTty;
}