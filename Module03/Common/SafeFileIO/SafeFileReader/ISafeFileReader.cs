using System;

namespace Common.SafeFileIO.SafeFileReader
{
    public interface ISafeFileReader : IDisposable
    {
        int Read(byte[] buffer, int offset, int count);
        long Length { get; }
    }
}