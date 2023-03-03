using System.Runtime.CompilerServices;
using System.Text;

namespace Tutu.Unix.Extensions;

internal static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ToPathname(this string path)
    {
        var byteLength = Encoding.UTF8.GetByteCount(path) + 1;
        var bytes = new byte[byteLength];
        Encoding.UTF8.GetBytes(path, bytes);
        return bytes;
    }
}
