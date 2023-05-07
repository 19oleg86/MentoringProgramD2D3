using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using Common.Logger;
using Common.MessageBroker;

namespace MainProcessingService
{
    static class MPService
    {
        private const string TARGETFILEDIRECTORY = "\\TargetFileDirectory";

        private static ILogger Logger { get; } = new ConsoleLogger();
        private static readonly ConcurrentDictionary<string, FileSaveHandler> Handlers = new ConcurrentDictionary<string, FileSaveHandler>();


        private static async void StartFileReceiving(string fileId, FileSaveHandler saveHandler)
        {
            await saveHandler.ProcessAsync();
            Handlers.TryRemove(fileId, out _);
        }

        private static ISimpleMessageReceiver GetFileReceiver()
        {
            var fileReceiver = new FileReceiver(new SimpleMessageBrokerProperties
            {
                Protocol = "amqp",
                Host = "127.0.0.1",
                Port = "5672",
                Login = "guest",
                Password = "guest"
            });
            fileReceiver.Received += OnReceiverReceived;

            try
            {
                fileReceiver.Initialize();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Can initialize FileReceiver => {ex.Message}");
                return null;
            }

            return fileReceiver;
        }

        private static void Main()
        {
            var fileReceiver = GetFileReceiver();


            if (fileReceiver == null)
            {
                Logger.LogWarn("Can not start receiving files.");
                Console.ReadLine();
                return;
            }

            using (fileReceiver)
            {
                Console.ReadLine();
            }
        }

        private static void OnReceiverReceived(object sender, ReceivedMessageDataEventArgs args)
        {
            var fileId = Encoding.ASCII.GetString(args.Properties["id"] as byte[] ?? Array.Empty<byte>());
            var fileName = Encoding.UTF8.GetString(args.Properties["file-name"] as byte[] ?? Array.Empty<byte>());
            var fileChunkNumber = (decimal)(args.Properties["file-chunk-number"] ?? 0);
            var totalFileChunks = (decimal)(args.Properties["total-file-chunks"] ?? 0);

            if (fileChunkNumber > 1 && !Handlers.TryGetValue(fileId, out _))
            {
                // We don't want to process files that didn't come from the very beginning
                Logger.LogWarn($"Received unknown data [file:\"{fileName}\" chunk:{fileChunkNumber}/{totalFileChunks}], skipped.");
                args.IsHandled = true;
                return;
            }

            var handler = Handlers.GetOrAdd(fileId, (id) =>
            {
                var workingDirectory = Environment.CurrentDirectory;
                var projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
                var homeFilePath = $"{projectDirectory}{TARGETFILEDIRECTORY}\\";
                var filePath = $"{homeFilePath}{DateTime.Now.Ticks}_{fileName}";

                var fileSaveHandler = new FileSaveHandler(filePath, Logger);
                StartFileReceiving(id, fileSaveHandler);
                return fileSaveHandler;
            });
            handler.AddData(args.Data);
            Logger.LogInfo($"Received {fileName,-60} => {fileChunkNumber}/{totalFileChunks}");

            args.IsHandled = true;
        }
    }
}
