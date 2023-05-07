using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.Logger;
using Common.MessageBroker;
using Common.SafeFileIO.SafeFileReader;

namespace DataCaptureService
{
    static class DCService
    {
        private const string DATACAPTUREDIRECTORY = "\\DataCaptureDirectory";
        private const int MAX_MESSAGE_FILE_CHUNK_LENGTH_IN_BYTES = 5;
        private static ILogger _logger { get; } = new ConsoleLogger();


        private static void Main()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            using (var watcher = new FileSystemWatcher($"{projectDirectory}{DATACAPTUREDIRECTORY}", "*.pdf"))
            {
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
                watcher.Created += OnFileAdded;
                watcher.EnableRaisingEvents = true;

                _logger.LogInfo(
                    $"To start working with Data Capture Service add some files in \"DataCaptureDirectoty\". {Environment.NewLine}" +
                    "Pay attention that only .pdf files will be processed and send to the queue");

                _logger.LogInfo("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static ISimpleMessageSender GetFileSender()
        {
            var fileSender = new FileSender(new SimpleMessageBrokerProperties
            {
                Protocol = "amqp",
                Host = "127.0.0.1",
                Port = "5672",
                Login = "guest",
                Password = "guest"
            });

            try
            {
                fileSender.Initialize();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can not initialize FileSender => {ex.Message}");
                fileSender.Dispose();
                return null;
            }

            return fileSender;
        }

        private static void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;
            Task.Factory.StartNew(() => SendFile(filePath));
        }

        private static bool SendFileChunk(ISimpleMessageSender fileSender, FileChunkInfo chunkInfo)
        {
            try
            {
                fileSender.SendMessage(chunkInfo.ChunkData, new Dictionary<string, object>
                {
                    { "id", chunkInfo.Id },
                    { "file-name", chunkInfo.FileName },
                    { "file-size", chunkInfo.FileSize },
                    { "file-chunk-number", chunkInfo.ChunkNumber },
                    { "total-file-chunks", chunkInfo.TotalChunks }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Can not send chunk number = {chunkInfo.ChunkNumber} of the file \"{chunkInfo.FileName}\" => {ex.Message}.");
                return false;
            }

            return true;
        }

        private static bool TryOpenFileReader(string filePath, out ISafeFileReader fileReader)
        {
            fileReader = null;
            try
            {
                fileReader = SafeFileReader.Create(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Can not Create or Open the file \"{filePath}\" => {ex.Message}");
                return false;
            }

            return true;
        }

        private static void SendFile(string filePath)
        {
            if (!TryOpenFileReader(filePath, out var fileReader))
            {
                _logger.LogWarn($"Can not open file \"{filePath}\".");
                return;
            }

            var fileSender = GetFileSender();
            if (fileSender == null)
            {
                _logger.LogWarn($"FileSender is not initialized, can not send file \"{filePath}\".");
                return;
            }

            using (fileReader)
            {
                var fileName = Path.GetFileName(filePath);
                var fileSize = new FileInfo(filePath).Length;
                
                using (fileSender)
                {
                    var totalChunks = Math.Ceiling(fileReader.Length / (decimal)MAX_MESSAGE_FILE_CHUNK_LENGTH_IN_BYTES);
                    if (totalChunks == 0)
                    {
                        _logger.LogWarn($"File \"{filePath}\" is empty, skiped.");
                        return;
                    }

                    var fileData = new byte[MAX_MESSAGE_FILE_CHUNK_LENGTH_IN_BYTES];
                    var fileId = Guid.NewGuid().ToString("N");
                    var fileChunkNumber = 0M;

                    var fileChunkInfo = new FileChunkInfo
                    {
                        Id = fileId,
                        FileName = fileName,
                        FileSize = fileSize,
                        //ChunkNumber =
                        //ChunkData = 
                        TotalChunks = totalChunks
                    };

                    var bytesRead = fileReader.Read(fileData, 0, fileData.Length);
                    while (bytesRead > 0)
                    {
                        var chunkData = new byte[bytesRead];
                        Array.Copy(fileData, chunkData, bytesRead);

                        fileChunkInfo.ChunkNumber = ++fileChunkNumber;
                        fileChunkInfo.ChunkData = chunkData;
                        var sendFileChunkResult = SendFileChunk(fileSender, fileChunkInfo);
                        if (!sendFileChunkResult)
                        {
                            _logger.LogWarn($"{fileName} is skipped, can not send chunk number = {fileChunkNumber}.");
                            break;
                        }

                        _logger.LogInfo($"Sent {fileName,-60} => {fileChunkNumber}/{totalChunks}");

                        //Simulation of a long process
                        Thread.Sleep(200);

                        bytesRead = fileReader.Read(fileData, 0, fileData.Length);
                    }
                }
            }
        }
    }
}
