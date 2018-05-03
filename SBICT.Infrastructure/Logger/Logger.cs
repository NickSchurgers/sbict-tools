using System;
using SBICT.Data.Enums;
using SBICT.Data.Models;

namespace SBICT.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private void Log(string message, LogLevel logLevel)
        {
            var args = new LoggerEventArgs
            {
                Log = new Log
                {
                    Message = message,
                    DateTime = DateTime.Now,
                    LogLevel = logLevel
                }
            };
            OnLogAdded(args);
        }

        public void Info(string message)
        {
            Log(message, LogLevel.Info);
        }

        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        public void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public event EventHandler<LoggerEventArgs> LogAdded;

        protected virtual void OnLogAdded(LoggerEventArgs e)
        {
            LogAdded?.Invoke(this, e);
        }
    }
}