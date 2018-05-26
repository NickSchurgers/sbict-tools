// <copyright file="ILogger.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Logger
{
    using System;

    public interface ILogger
    {
        event EventHandler<LoggerEventArgs> LogAdded;
        void Info(string message);
        void Debug(string message);
        void Error(string message);
        void Warning(string message);
    }
}