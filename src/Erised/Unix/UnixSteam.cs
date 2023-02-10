//
// Mono.Unix/UnixStream.cs
//
// Authors:
//   Jonathan Pryor (jonpryor@vt.edu)
//
// (C) 2004-2006 Jonathan Pryor
// (C) 2007 Novell, Inc.
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Erised;
using Tmds.Linux;
using static Tmds.Linux.LibC;

namespace Mono.Unix;

public sealed class UnixStream : Stream, IDisposable
{
    public const int InvalidFileDescriptor = -1;
    public const int StandardInputFileDescriptor = 0;
    public const int StandardOutputFileDescriptor = 1;
    public const int StandardErrorFileDescriptor = 2;

    private readonly bool _owner;
    private int _fileDescriptor = InvalidFileDescriptor;
    private stat _stat;

    public UnixStream(int fileDescriptor)
        : this(fileDescriptor, true)
    {
    }

    public unsafe UnixStream(int fileDescriptor, bool ownsHandle)
    {
        if (InvalidFileDescriptor == fileDescriptor)
            throw new ArgumentException("Invalid file descriptor", nameof(fileDescriptor));

        this._fileDescriptor = fileDescriptor;
        _owner = ownsHandle;

        long offset = lseek(fileDescriptor, 0, SEEK_CUR);
        if (offset != -1)
        {
            CanSeek = true;
        }

        var empty = Array.Empty<byte>();
        fixed (byte* ptr = empty)
        {
            long read = LibC.read(fileDescriptor, ptr, 0);
            if (read != -1)
            {
                CanRead = true;
            }
        }

        fixed (byte* ptr = empty)
        {
            long write = LibC.write(fileDescriptor, ptr, 0);
            if (write != -1)
            {
                CanWrite = true;
            }
        }
    }

    private void AssertNotDisposed()
    {
        if (_fileDescriptor == InvalidFileDescriptor)
            throw new ObjectDisposedException("Invalid File Descriptor");
    }

    public int Handle => _fileDescriptor;

    public override bool CanRead { get; }

    public override bool CanSeek { get; }

    public override bool CanWrite { get; }

    public override long Length
    {
        get
        {
            AssertNotDisposed();
            if (!CanSeek)
                throw new NotSupportedException("File descriptor doesn't support seeking");
            RefreshStat();
            return _stat.st_size;
        }
    }

    public override long Position
    {
        get
        {
            AssertNotDisposed();
            if (!CanSeek)
            {
                throw new NotSupportedException("The stream does not support seeking");
            }

            long pos = lseek(_fileDescriptor, 0, SEEK_CUR);
            if (pos == -1)
            {
                throw new PlatformException();
            }

            return pos;
        }
        set => Seek(value, SeekOrigin.Begin);
    }

    public long OwnerGroupId
    {
        get
        {
            RefreshStat();
            return _stat.st_gid;
        }
    }

    private unsafe void RefreshStat()
    {
        AssertNotDisposed();
        int result;
        fixed (stat* s = &_stat)
        {
            result = fstat(_fileDescriptor, s);
        }

        if (result != 0)
        {
            throw new PlatformException();
        }
    }

    public override void Flush()
    {
    }

    public override unsafe int Read([In, Out] byte[] buffer, int offset, int count)
    {
        AssertNotDisposed();
        AssertValidBuffer(buffer, offset, count);
        if (!CanRead)
            throw new NotSupportedException("Stream does not support reading");

        if (buffer.Length == 0)
            return 0;

        long r;
        fixed (byte* buf = &buffer[offset])
        {
            do
            {
                r = read(_fileDescriptor, buf, count);
            } while (ShouldRetrySyscall((int)r));
        }

        if (r == -1 && errno != 0)
        {
            throw new PlatformException();
        }

        return (int)r;
    }

    private static bool ShouldRetrySyscall(int r) => r == -1 && errno == EINTR;

    private static void AssertValidBuffer(byte[] buffer, int offset, int count)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "< 0");
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "< 0");
        }

        if (offset > buffer.Length)
        {
            throw new ArgumentException("destination offset is beyond array size");
        }

        if (offset > buffer.Length - count)
        {
            throw new ArgumentException("would overrun buffer");
        }
    }


    [DoesNotReturn]
    private static void ThrowExceptionForLastError()
        => throw new PlatformException();


    public override long Seek(long offset, SeekOrigin origin)
    {
        AssertNotDisposed();
        if (!CanSeek)
        {
            throw new NotSupportedException("The File Descriptor does not support seeking");
        }

        var sf = origin switch
        {
            SeekOrigin.Begin => SEEK_SET,
            SeekOrigin.Current => SEEK_CUR,
            SeekOrigin.End => SEEK_END,
            _ => SEEK_CUR
        };

        long pos = lseek(_fileDescriptor, offset, sf);
        if (pos == -1)
        {
            ThrowExceptionForLastError();
        }

        return pos;
    }

    public override void SetLength(long value)
    {
        AssertNotDisposed();
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "< 0");
        }

        if (!CanSeek && !CanWrite)
        {
            throw new NotSupportedException("You can't truncating the current file descriptor");
        }

        int r;
        do
        {
            r = ftruncate(_fileDescriptor, value);
        } while (ShouldRetrySyscall(r));

        if (r == -1)
        {
            ThrowExceptionForLastError();
        }
    }

    public override unsafe void Write(byte[] buffer, int offset, int count)
    {
        AssertNotDisposed();
        AssertValidBuffer(buffer, offset, count);
        if (!CanWrite)
        {
            throw new NotSupportedException("File Descriptor does not support writing");
        }

        if (buffer.Length == 0)
        {
            return;
        }

        long r;
        fixed (byte* buf = &buffer[offset])
        {
            do
            {
                r = write(_fileDescriptor, buf, count);
            } while (ShouldRetrySyscall((int)r));
        }

        if (r == -1)
        {
            ThrowExceptionForLastError();
        }
    }

    public unsafe void WriteAtOffset(byte[] buffer,
        int offset, int count, long fileOffset)
    {
        AssertNotDisposed();
        AssertValidBuffer(buffer, offset, count);
        if (!CanWrite)
        {
            throw new NotSupportedException("File Descriptor does not support writing");
        }

        if (buffer.Length == 0)
        {
            return;
        }

        long r = 0;
        fixed (byte* buf = &buffer[offset])
        {
            do
            {
                r = pwrite(_fileDescriptor, buf, count, fileOffset);
            } while (ShouldRetrySyscall((int)r));
        }

        if (r == -1)
        {
            ThrowExceptionForLastError();
        }
    }

    ~UnixStream()
    {
        Close();
    }

    public override void Close()
    {
        if (_fileDescriptor == InvalidFileDescriptor)
        {
            return;
        }

        Flush();

        if (!_owner)
        {
            return;
        }

        int r;
        do
        {
            r = close(_fileDescriptor);
        } while (ShouldRetrySyscall(r));

        if (r == -1)
        {
            ThrowExceptionForLastError();
        }

        _fileDescriptor = InvalidFileDescriptor;
        GC.SuppressFinalize(this);
    }

    void IDisposable.Dispose()
    {
        if (_fileDescriptor != InvalidFileDescriptor && _owner)
        {
            Close();
        }

        GC.SuppressFinalize(this);
    }
}