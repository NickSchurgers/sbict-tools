// <copyright file="LoggerEventArgs.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Logger
{
    /// <summary>
    /// Event args for logger related events.
    /// </summary>
    public class LoggerEventArgs
    {
        /// <summary>
        /// Log entry the event applies to.
        /// </summary>
        public Log Log { get; set; }
    }
}