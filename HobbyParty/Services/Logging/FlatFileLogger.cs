using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logging
{
    public class FlatFileLogger:ILogger 
    {
        public async Task<bool> LogAsync(LogMessage message)
        {
            
        }

    }
}
