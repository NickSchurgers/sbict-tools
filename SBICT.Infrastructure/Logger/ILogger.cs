// <copyright file="ILogger.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Logger
{
    /// <summary>
    /// Writes messages to files.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log item with loglevel info.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void Info(string message);

        /// <summary>
        /// Log item with loglevel debug.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void Debug(string message);

        /// <summary>
        /// Log item with loglevel error.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void Error(string message);

        /// <summary>
        /// Log item with loglevel warning.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void Warning(string message);
    }
}