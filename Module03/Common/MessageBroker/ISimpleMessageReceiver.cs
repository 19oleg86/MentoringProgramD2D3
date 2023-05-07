using System;

namespace Common.MessageBroker
{

    public interface ISimpleMessageReceiver : IDisposable
    {
        void Initialize();
        event EventHandler<ReceivedMessageDataEventArgs> Received;
    }
}
