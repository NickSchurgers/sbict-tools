namespace SBICT.Infrastructure.Logger
{
    using System;

    public class Log
    {
        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Message { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}