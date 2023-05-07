using System;
using System.Collections.Generic;

namespace Common.MessageBroker
{
    public class ReceivedMessageDataEventArgs : EventArgs
    {
        public byte[] Data { get; }
        public IDictionary<string, object> Properties { get; }
        public bool IsHandled { get; set; }

        public ReceivedMessageDataEventArgs(byte[] data, IDictionary<string, object> properties)
        {
            Data = data;
            Properties = properties;
        }
    }
}
