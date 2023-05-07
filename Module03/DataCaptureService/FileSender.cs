using RabbitMQ.Client;
using System.Collections.Generic;
using Common.MessageBroker;
using Common.MessageBroker.RabbitMq;

namespace DataCaptureService
{
    public class FileSender : SimpleRabbitMqMessageSender
    {
        protected override string ExchangeType => RabbitMQ.Client.ExchangeType.Headers;
        protected override string ExchangeName => "FileTransferExchange";

        public FileSender(SimpleMessageBrokerProperties props) 
            : base(props)
        {
        }

        protected override IBasicProperties GetProperties(IDictionary<string, object> messageAttributes)
        {
            var properties = base.GetProperties(messageAttributes);

            var messageHeaders = new Dictionary<string, object>
            {
                { "message-type", "send-file" }
            };

            if (messageAttributes != null)
            {
                foreach (var attr in messageAttributes)
                {
                    messageHeaders.Add(attr.Key, attr.Value);
                }
            }
            properties.Headers = messageHeaders;

            return properties;
        }

    }
}