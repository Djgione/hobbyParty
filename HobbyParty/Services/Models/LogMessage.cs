using Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class LogMessage
    {
        public string Timestamp { get; set; } 
        public LogCategories Category { get; set; }
        public string Message { get; set; }

        public LogMessage()
        { }
        public LogMessage(string timestamp, LogCategories category, string message)
        {
            Timestamp = timestamp;
            Category = category;
            Message = message;
        }

        public override string ToString()
        {
            return Timestamp + " " + Category + ": " + Message;
        }
    }
}
