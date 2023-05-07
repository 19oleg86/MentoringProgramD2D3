using System;
using System.Collections.Generic;

namespace Common.MessageBroker
{
    public interface ISimpleMessageSender : IDisposable
    {
        void SendMessage(byte[] message, IDictionary<string, object> attributes = null);
        void Initialize();
    }
}
