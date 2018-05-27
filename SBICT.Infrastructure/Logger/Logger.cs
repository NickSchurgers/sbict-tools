// <copyright file="Logger.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Logger
{
    /// <inheritdoc />
    public class Logger : ILogger
    {
        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="logLevel">Level of the message.</param>
        private void Log(string message, LogLevel logLevel)
        {
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.Log(message, LogLevel.Info);
        }

        /// <inheritdoc/>
        public void Debug(string message)
        {
            this.Log(message, LogLevel.Debug);
        }

        /// <inheritdoc/>
        public void Error(string message)
        {
            this.Log(message, LogLevel.Error);
        }

        /// <inheritdoc/>
        public void Warning(string message)
        {
            this.Log(message, LogLevel.Warning);
        }
    }
}