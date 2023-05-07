using System.Collections.Generic;
using Common.MessageBroker;
using Common.MessageBroker.RabbitMq;

namespace MainProcessingService
{
    public class FileReceiver : SimpleRabbitMqMessageReceiver
    {
        protected override string ExchangeName => "FileTransferExchange";
        protected override string QueueName => "FileTransferQueue";

        protected override string ExchangeType => RabbitMQ.Client.ExchangeType.Headers;

        public FileReceiver(SimpleMessageBrokerProperties props) 
            : base(props)
        {
        }

        protected override IDictionary<string, object> GetQueueBindArguments()
        {
            return new Dictionary<string, object>
            {
                { "x-match", "all" },
                { "message-type", "send-file" }
            };
        }
    }
}