using System;

namespace SBICT.Data
{
    public struct Message : IMessage
    {
        public string Content { get; }
        public DateTime Received { get; }

        public Message(string content)
        {
            Content = content;
            Received = DateTime.Now;
        }
    }
}