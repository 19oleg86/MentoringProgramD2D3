using System.Collections.Generic;
using System.Threading.Channels;
using RabbitMQ.Client;

namespace Common.MessageBroker.RabbitMq
{

    public abstract class SimpleRabbitMqMessageSender : SimpleRabbitMqBase, ISimpleMessageSender
    {
        protected abstract string ExchangeName { get; }

        protected abstract string ExchangeType { get; }

        protected virtual bool IsExchangeDurable { get; } = true;
        protected virtual bool AutoDeleteExchange { get; } = false;
        protected virtual string RoutingKey { get; } = string.Empty;

        protected SimpleRabbitMqMessageSender(SimpleMessageBrokerProperties props)
            : base(props)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Channel.ExchangeDeclare(ExchangeName, ExchangeType, IsExchangeDurable, AutoDeleteExchange);
        }

        protected virtual IBasicProperties GetProperties(IDictionary<string, object> messageAttributes)
        {
            return Channel.CreateBasicProperties();
        }

        public virtual void SendMessage(byte[] message, IDictionary<string, object> attributes = null)
        {
            if (Channel == null)
            {
                throw new ChannelClosedException("Can not send a message when connection is closed. Call the Initialize method before.");
            }

            Channel.BasicPublish(ExchangeName, RoutingKey, GetProperties(attributes), message);
        }
    }
}
