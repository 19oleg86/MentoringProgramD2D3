using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCaptureService
{
    class DCService
    {
        private const string DATACAPTUREDIRECTORY = "\\DataCaptureDirectory";
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

                Console.WriteLine($"To start working with Data Capture Service add some files in \"DataCaptureDirectoty\". {Environment.NewLine} " +
                    $"Pay attention that only .pdf files will be processed and send to the queue");

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static void OnFileAdded(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.EndsWith(".pdf"))
            {
                Console.WriteLine("Added file is in pdf format");
                Console.WriteLine($"File's full name with path: {e.FullPath}");
                var factory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://guest:guest@localhost:5672")
                };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare("webappExchange", ExchangeType.Direct, true);
                byte[] fileBytes = File.ReadAllBytes(e.FullPath);
                channel.BasicPublish("webappExchange", "file.pdf", null, fileBytes);

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
            channel.ExchangeDeclare("webappExchange", ExchangeType.Direct, true);
            channel.Close();
            connection.Close();
        }
    }
}
