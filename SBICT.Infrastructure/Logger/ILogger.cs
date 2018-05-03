using System;
using SBICT.Data.Enums;

namespace SBICT.Infrastructure.Logger
{
    public interface ILogger
    {
        event EventHandler<LoggerEventArgs> LogAdded;
        void Info(string message);
        void Debug(string message);
        void Error(string message);
        void Warning(string message);
    }
}