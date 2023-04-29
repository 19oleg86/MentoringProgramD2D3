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
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("addFileQueue", true, false, false);
            channel.QueueBind("addFileQueue", "addFileExchange", "file.pdf");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                //var msg = System.Text.Encoding.UTF8.GetString(eventArgs.Body.ToArray()); eventArgs.Body.ToArray()
                var fileBytes = eventArgs.Body.ToArray();
                File.WriteAllBytes($"{filePath}file.pdf", fileBytes);
                Console.WriteLine($"Exchange routing key: {eventArgs.RoutingKey}, Target file path: {filePath}file.pdf");
            };
            channel.BasicConsume("addFileQueue", true, consumer);

            Console.ReadLine();

            channel.Close();
            connection.Close();
        }
    }
}
