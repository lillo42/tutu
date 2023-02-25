using System.Runtime.InteropServices;

namespace Tutu.Windows.Interop;

internal static partial class Windows
{
    public static partial class Interop
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public uint nLength;
            public nint lpSecurityDescriptor;
            [MarshalAs(UnmanagedType.Bool)] public bool bInheritHandle;
        }
    }
}