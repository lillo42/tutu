using System.Text;
using Tmds.Linux;

namespace Tutu.Unix;

internal static partial class Unix
{
    /// <summary>
    /// A file descriptor wrapper.
    ///
    /// It allows to retrieve raw file descriptor, write to the file descriptor and
    /// mainly it closes the file descriptor once dropped.
    /// </summary>
    /// <param name="Fd"></param>
    /// <param name="CloseOnDispose"></param>
    public readonly record struct FileDesc(int Fd, bool CloseOnDispose) : IDisposable
    {
        /// <summary>
        /// Creates a file descriptor pointing to the standard input or `/dev/tty`.
        /// </summary>
        /// <returns></returns>
        public static unsafe FileDesc TtyFd()
        {
            var fd = LibC.isatty(LibC.STDIN_FILENO);
            if (fd == 1)
            {
                return new(LibC.STDIN_FILENO, false);
            }

            var path = "/dev/tty";
            var byteLength = Encoding.UTF8.GetByteCount(path) + 1;
            var bytes = byteLength <= 128 ? stackalloc byte[byteLength] : new byte[byteLength];
            Encoding.UTF8.GetBytes(path, bytes);

            fixed (byte* ptr = bytes)
            {
                fd = LibC.open(ptr, LibC.O_RDWR);
            }

            return new FileDesc(fd, true);
        }

        public unsafe uint Read(byte[] buffer, uint size)
        {
            ssize_t read;
            fixed (void* ptr = buffer)
            {
                read = LibC.read(Fd, ptr, size);
            }

            if (read < 0)
            {
                PlatformException.Throw();
            }

            return (uint)read;
        }

        public void Dispose()
        {
            if (CloseOnDispose)
            {
                LibC.close(Fd);
            }
        }
    }
}