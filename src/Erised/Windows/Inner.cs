namespace Erised;

internal static partial class Windows
{
    /// <summary>
    /// Inner structure for closing a handle on Drop.
    ///
    /// The second parameter indicates if the HANDLE is exclusively owned or not.
    /// A non-exclusive handle can be created using for example
    /// `Handle::input_handle` or `Handle::output_handle`, which corresponds to
    /// stdin and stdout respectively.
    /// </summary>
    /// <param name="Handler"></param>
    /// <param name="IsExclusive"></param>
    public record Inner(nint Handler, bool IsExclusive) : IDisposable
    {
        public void Dispose()
        {
            if (IsExclusive)
            {
                Interop.Kernel32.CloseHandle(Handler);
            }
        }
    }
}