using System.IO;

namespace Common.SafeFileIO.SafeFileWriter
{
    public class SafeFileWiter: ISafeFileWiter
    {
        private readonly Stream _fileStream;

        public static ISafeFileWiter Create(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            return new SafeFileWiter(filePath, mode, access, share);
        }

        private SafeFileWiter(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            _fileStream = new FileStream(filePath, mode, access, share);
        }

        public bool TryAppend(byte[] data)
        {
            try
            {
                _fileStream.Seek(0, SeekOrigin.End);
                _fileStream.Write(data, 0, data.Length);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool TrySave()
        {
            try
            {
                _fileStream.Flush();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool TryClose()
        {
            try
            {
                Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }
    }
}