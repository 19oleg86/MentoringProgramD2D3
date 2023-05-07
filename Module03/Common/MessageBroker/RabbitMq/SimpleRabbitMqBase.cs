using System;
using RabbitMQ.Client;

namespace Common.MessageBroker.RabbitMq
{

    public abstract class SimpleRabbitMqBase : IDisposable
    {
        private readonly ConnectionFactory _factory;
        protected IModel Channel { get; private set; }
        private IConnection _connection;


        protected SimpleRabbitMqBase(SimpleMessageBrokerProperties props)
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri($"{props.Protocol}://{props.Login}:{props.Password}@{props.Host}:{props.Port}")
            };
        }

        public virtual void Initialize()
        {
            _connection = _factory.CreateConnection();
            Channel = _connection.CreateModel();
        }

        public virtual void Dispose()
        {
            Channel?.Close();
            _connection?.Close();
            Channel?.Dispose();
            _connection?.Dispose();
        }

    }
}
