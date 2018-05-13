using System;

namespace SBICT.Data
{
    public interface IMessage
    {
        string Content { get; }
        DateTime Received { get; }
    }
}