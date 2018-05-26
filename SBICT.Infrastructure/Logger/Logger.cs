namespace SBICT.Infrastructure.Logger
{
    using System;

    public class Logger : ILogger
    {
        #region Methods

        /// <summary>
        /// Log item
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
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

        /// <summary>
        /// Log item with loglevel info
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            Log(message, LogLevel.Info);
        }

        /// <summary>
        /// Log item with loglevel debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        /// <summary>
        /// Log item with loglevel error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            Log(message, LogLevel.Error);
        }

        /// <summary>
        /// Log item with loglevel warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        #endregion

        #region Events

        public event EventHandler<LoggerEventArgs> LogAdded;

        #endregion

        #region Event Handlers

        protected virtual void OnLogAdded(LoggerEventArgs e)
        {
            LogAdded?.Invoke(this, e);
        }

        #endregion
    }
}