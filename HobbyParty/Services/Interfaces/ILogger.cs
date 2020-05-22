using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    interface ILogger
    {
        Task<bool> LogAsync(LogMessage message);
    }
}
