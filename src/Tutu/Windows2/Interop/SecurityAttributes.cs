using System.Runtime.InteropServices;

namespace Tutu.Windows2.Interop;

[StructLayout(LayoutKind.Sequential)]
internal struct SECURITY_ATTRIBUTES
{
    public int nLength;
    public nint lpSecurityDescriptor;
    
    [MarshalAs(UnmanagedType.Bool)] 
    public bool bInheritHandle;
}