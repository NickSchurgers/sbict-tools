// <copyright file="SystemLogger.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
    using Prism.Events;
    using SBICT.Infrastructure.Logger;

    /// <summary>
    /// Class used to easily publish messages to the system log window.
    /// </summary>
    public static class SystemLogger
    {
        /// <summary>
        /// Sets instance of aggregator required to publish the log events.
        /// </summary>
        public static IEventAggregator EventAggregator { private get; set; }

        /// <summary>
        /// Publish an event to the systemlog.
        /// </summary>
        /// <param name="message">Message to push to the system log.</param>
        /// <param name="logLevel">Type of message.</param>
        public static void LogEvent(string message, LogLevel logLevel = LogLevel.Info)
        {
            EventAggregator?.GetEvent<SystemLogEvent>().Publish(new Log { Message = message, LogLevel = logLevel });
        }
    }
}