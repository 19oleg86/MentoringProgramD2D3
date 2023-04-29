using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCaptureService
{
    class DCService
    {
        private const string DATACAPTUREDIRECTORY = "\\DataCaptureDirectory";
        private const int HUNDRED_MB_IN_BYTES = 104857600;
        static void Main(string[] args)
        {
            CreateExchange();
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            using (var watcher = new FileSystemWatcher($"{projectDirectory}{DATACAPTUREDIRECTORY}"))
            {
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
                watcher.Created += OnFileAdded;
                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"To start working with Data Capture Service add some files in \"DataCaptureDirectoty\". {Environment.NewLine}" +
                    $"Pay attention that only .pdf files will be processed and send to the queue");

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created && e.FullPath.EndsWith(".pdf"))
            {
                string fileName = e.FullPath.Substring(e.FullPath.LastIndexOf('\\') + 1);
                long fileSize = new FileInfo(e.FullPath).Length;

                bool fileAvailable = false;
                Console.WriteLine("Added file is in pdf format");
                Console.WriteLine($"File's full name with path: {e.FullPath}");
                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest:guest@localhost:5672")
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("addFileExchange", ExchangeType.Headers, true);
                while (!fileAvailable)
                {
                    try
                    {
                        byte[] fileBytes = File.ReadAllBytes(e.FullPath);
                        fileAvailable = true;

                        if (fileSize >= HUNDRED_MB_IN_BYTES)
                        {
                            Console.WriteLine($"Created file \"{e.Name}\" has 100 Mb or bigger size");
                            var fileStream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read);
                            var bigSizeheaders = new Dictionary<string, object>
                            {
                                { "fileName", fileName },
                                { "size", "big" },
                            };

                            int chunkNumber = 1;
                            byte[] buffer = new byte[52428800]; // 50MB
                            while (fileStream.Position < fileStream.Length)
                            {
                                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                byte[] chunkData = new byte[bytesRead];
                                Array.Copy(buffer, chunkData, bytesRead);
                                var bigSizeProperties = channel.CreateBasicProperties();
                                bigSizeProperties.Headers = bigSizeheaders;
                                if (!bigSizeProperties.Headers.ContainsKey("chunknumber"))
                                    bigSizeProperties.Headers.Add("chunknumber", chunkNumber.ToString());
                                bigSizeProperties.Headers["chunknumber"] = chunkNumber.ToString();

                                channel.BasicPublish("addFileExchange", string.Empty, bigSizeProperties, chunkData);
                                Console.WriteLine($"Sent chunk {chunkNumber}");
                                chunkNumber++;
                            }
                            fileStream.Close();
                        }
                        else
                        {
                            var headers = new Dictionary<string, object>
                            {
                                { "fileName", fileName },
                                { "size", "normal" }
                            };
                            var properties = channel.CreateBasicProperties();
                            properties.Headers = headers;
                            channel.BasicPublish("addFileExchange", string.Empty, properties, fileBytes);
                        }

                    }
                    catch (IOException)
                    {
                        Console.WriteLine($"File {e.FullPath} is not available yet. Retrying in 1 second.");
                        Thread.Sleep(1000); // wait for 1 second before trying again
                    }
                }
                channel.Close();
                connection.Close();
            }
        }

        private static void CreateExchange()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("addFileExchange", ExchangeType.Headers, true);
            channel.Close();
            connection.Close();
        }
    }
}
