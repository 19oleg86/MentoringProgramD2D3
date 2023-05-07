using System;
using System.IO;

namespace Common.SafeFileIO.SafeFileReader
{
    public class SafeFileReader:ISafeFileReader
    {
        public static ISafeFileReader Create(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            return new SafeFileReader(filePath, mode, access, share);
        }

        private readonly Stream _fileStream;

        public long Length
        {
            get
            {
                try
                {
                    return _fileStream.Length;
                }
                catch
                {
                    return 0;
                }
            }
        }

        private SafeFileReader(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            _fileStream = new FileStream(filePath, mode, access, share);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                return _fileStream.Read(buffer, offset, count);
            }
            catch
            {
            }

            return 0;
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }
    }
}