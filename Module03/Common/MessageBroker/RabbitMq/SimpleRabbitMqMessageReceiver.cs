using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.MessageBroker.RabbitMq
{
    public abstract class SimpleRabbitMqMessageReceiver : SimpleRabbitMqBase, ISimpleMessageReceiver
    {

        protected abstract string ExchangeType { get; }
        protected virtual bool IsExchangeDurable { get; } = true;
        protected virtual bool AutoDeleteExchange { get; } = false;

        protected abstract string ExchangeName { get; }
        protected abstract string QueueName { get; }

        protected virtual bool IsQueueDurable { get; } = true;
        protected virtual bool IsExclusiveQueue { get; } = false;
        protected virtual bool AutoDeleteQueue { get; } = false;
        protected virtual string RoutingKey { get; } = string.Empty;


        private EventingBasicConsumer _consumer;
        public event EventHandler<ReceivedMessageDataEventArgs> Received;
        private void OnReceived(ReceivedMessageDataEventArgs e)
        {
            Received?.Invoke(this, e);
        }

        protected SimpleRabbitMqMessageReceiver(SimpleMessageBrokerProperties props)
            : base(props)
        {
        }

        protected virtual IDictionary<string, object> GetQueueBindArguments()
        {
            return null;
        }

        public override void Initialize()
        {
            base.Initialize();
            Channel.ExchangeDeclare(ExchangeName, ExchangeType, IsExchangeDurable, AutoDeleteExchange);

            Channel.QueueDeclare(QueueName, IsQueueDurable, IsExclusiveQueue, AutoDeleteQueue);
            Channel.QueueBind(QueueName, ExchangeName, RoutingKey, GetQueueBindArguments());
            Channel.BasicQos(0, 1, true);

            _consumer = new EventingBasicConsumer(Channel);
            _consumer.Received += OnConsumerReceived;
            Channel.BasicConsume(QueueName, false, _consumer);
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs args)
        {
            var eventArgs = new ReceivedMessageDataEventArgs(args.Body.ToArray(), args.BasicProperties.Headers);
            OnReceived(eventArgs);
            if (eventArgs.IsHandled)
            {
                Channel.BasicAck(args.DeliveryTag, false);
            }
            else
            {
                //Something went wrong maybe we want to send this data to DLQ (Dead Letter Queue)
            }
        }

        public override void Dispose()
        {
            if (_consumer != null)
            {
                _consumer.Received -= OnConsumerReceived;
            }
            base.Dispose();
        }
    }
}
