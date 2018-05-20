﻿using System;
using SBICT.Data;

namespace SBICT.Infrastructure.Connection
{
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionStatus Status { get; set; }
        public IUser User { get; set; }
    }
}