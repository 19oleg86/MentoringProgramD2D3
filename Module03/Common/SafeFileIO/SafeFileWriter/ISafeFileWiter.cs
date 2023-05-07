using System;

namespace Common.SafeFileIO.SafeFileWriter
{
    public interface ISafeFileWiter : IDisposable
    {
        bool TryAppend(byte[] data);
        bool TrySave();
        bool TryClose();
    }
}