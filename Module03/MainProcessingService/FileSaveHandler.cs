using Common.SafeFileIO.SafeFileWriter;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.Logger;

namespace MainProcessingService
{
    public class FileSaveHandler
    {
        private readonly ConcurrentQueue<byte[]> _data;
        private readonly string _filePath;
        private ManualResetEventSlim _manualResetEvent;
        private readonly ILogger _logger;

        public FileSaveHandler(string filePath, ILogger logger)
        {
            _filePath = filePath;
            _data = new ConcurrentQueue<byte[]>();
            _logger = logger;
        }

        public void AddData(byte[] data)
        {
            _data.Enqueue(data);
            if(_manualResetEvent != null && !_manualResetEvent.IsSet)
            {
                _manualResetEvent.Set();
            }
        }

        private bool TryOpenFileWriter(out ISafeFileWiter fileWriter)
        {
            fileWriter = null;
            try
            {
                fileWriter = SafeFileWiter.Create(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Can not Create or Open the file \"{_filePath}\" => {ex.Message}");
                return false;
            }

            return true;
        }


        public Task ProcessAsync() =>
            Task.Factory.StartNew(() => {
                _manualResetEvent = new ManualResetEventSlim(true);
                do
                {
                    if(TryOpenFileWriter(out var fileWiter))
                    {
                        while (_data.TryDequeue(out var bytes))
                        {
                            if (!fileWiter.TryAppend(bytes))
                            {
                                _logger.LogWarn($"Can not append data to the file \"{_filePath}\".");
                            }

                            if (!fileWiter.TrySave())
                            {
                                _logger.LogWarn($"Can not save to the file \"{_filePath}\".");
                            }
                        }
                        fileWiter.TryClose();
                    }

                    _manualResetEvent.Reset();
                    _manualResetEvent.Wait(TimeSpan.FromSeconds(15));
                } while (_data.Count > 0);
                _logger.LogInfo($"Data save process to the file \"{_filePath}\" is completed.");
            });
    }
}
