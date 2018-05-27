// <copyright file="Log.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Logger
{
    using System;

    /// <summary>
    /// Instance of a log message.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Gets or sets date and time of log item.
        /// </summary>
        public DateTime DateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the content of the log item.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the level of the log item.
        /// </summary>
        public LogLevel LogLevel { get; set; }
    }
}