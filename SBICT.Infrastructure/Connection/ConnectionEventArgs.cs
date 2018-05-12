using System;
using SBICT.Data;

namespace SBICT.Infrastructure.Connection
{
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionStatus Status { get; set; }
        public User User { get; set; }
    }
}