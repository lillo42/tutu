using cc_t = System.Byte;
using speed_t = System.UInt32;
using tcflag_t = System.UInt32;
#pragma warning disable CS0169
#pragma warning disable CS0649
#pragma warning disable CS8981

namespace Tutu.Unix.Interop.LibC;

internal unsafe struct termios
{
    private const int NCCS = 32; // note: 16 on powerpc/powerpc64

    public tcflag_t c_iflag;
    public tcflag_t c_oflag;
    public tcflag_t c_cflag;
    public tcflag_t c_lflag;
    public cc_t c_line;
    public fixed cc_t c_cc[NCCS];
    private speed_t __c_ispeed;
    private speed_t __c_ospeed;
}
