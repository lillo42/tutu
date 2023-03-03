using Tmds.Linux;
using Tutu.Tty;

namespace Tutu.Unix;

/// <summary>
/// The Unix implementation of <see cref="ITty"/>. 
/// </summary>
public class UnixTty : ITty
{
    /// <inheritdoc cref="ITty.IsTty"/>
    public bool IsTty
    {
        get
        {
            var tty = FileDesc.TtyFd();
            return LibC.isatty(tty) == 1;
        }
    }
}
