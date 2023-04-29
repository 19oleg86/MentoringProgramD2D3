using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProcessingService
{
    class MPService
    {
        private const string TARGETFILEDIRECTORY = "\\TargetFileDirectory";
        static void Main(string[] args)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            string filePath = $"{projectDirectory}{TARGETFILEDIRECTORY}\\";
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("addFileQueue", true, false, false);
            channel.QueueBind("addFileQueue", "addFileExchange", string.Empty);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var fileName = Encoding.UTF8.GetString(eventArgs.BasicProperties.Headers["fileName"] as byte[]);
                var fileSize = Encoding.UTF8.GetString(eventArgs.BasicProperties.Headers["size"] as byte[]);
                if (fileSize == "normal")
                {
                    var fileBytes = eventArgs.Body.ToArray();
                    File.WriteAllBytes($"{filePath}{fileName}", fileBytes);
                    Console.WriteLine($"Target file path: {filePath}{fileName}");
                    channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                else if (fileSize == "big")
                {
                    var headers = eventArgs.BasicProperties.Headers;
                    var chunkNumber = Encoding.UTF8.GetString(eventArgs.BasicProperties.Headers["chunknumber"] as byte[]);
                    var chunkData = eventArgs.Body.ToArray();

                    AppendChunkToFile(chunkData, int.Parse(chunkNumber), $"{filePath}{fileName}");
                    channel.BasicAck(eventArgs.DeliveryTag, false);
                }
            };

            channel.BasicConsume("addFileQueue", true, consumer);

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();

            channel.Close();
            connection.Close();
        }

        private static void AppendChunkToFile(byte[] chunkData, int chunkNumber, string outputFile)
        {
            using (var fileStream = new FileStream(outputFile, chunkNumber == 1 ? FileMode.Create : FileMode.Append))
            {
                fileStream.Write(chunkData, 0, chunkData.Length);
            }
            Console.WriteLine($"Received chunk {chunkNumber}");
        }
    }
}
