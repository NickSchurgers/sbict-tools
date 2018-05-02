using System;

namespace SBICT.Infrastructure.Connection
{
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionStatus Status { get; set; }
        public string User { get; set; }
    }
}