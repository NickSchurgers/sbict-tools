using SBICT.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SBICT.Data.Models
{
    public class Log
    {
        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Message { get; set; }

        public LogLevel LogLevel { get; set; }
    }
}