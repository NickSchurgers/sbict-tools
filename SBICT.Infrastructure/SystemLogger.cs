using CommonServiceLocator;
using SBICT.Data.Enums;
using SBICT.Data.Models;
using SBICT.Infrastructure;
using Prism.Events;


namespace SBICT.Infrastructure
{
    public class SystemLogger
    {
        public static IEventAggregator EventAggregator { private get; set; }

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
    }
}