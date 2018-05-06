﻿using Prism.Events;
using SBICT.Infrastructure.Logger;


namespace SBICT.Infrastructure
{
    public class SystemLogger
    {
        #region Properties

        public static IEventAggregator EventAggregator { private get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Publish an event to the systemlog
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        public static void LogEvent(string message, LogLevel logLevel = LogLevel.Info)
        {
            EventAggregator?.GetEvent<SystemLogEvent>()
                .Publish(new Log {Message = message, LogLevel = logLevel});
        }

        #endregion
    }
}