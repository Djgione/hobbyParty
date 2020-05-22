using Services.Interfaces;
using Services.Logging;
using Services.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public static class Logger
    {
        private static readonly IEnumerable<ILogger> _loggers;

        /// <summary>
        /// Adds the necessary ILogger in order to create the collection properly
        /// </summary>
        static Logger()
        {
            _loggers = new List<ILogger> { new FlatFileLogger() };
        }
        
        public static async Task<Tuple<string,bool>> Log(object o)
        {
            


            var list = new List<Tuple<string, bool>>(); 
            foreach(ILogger logger in _loggers)
            {
                


            }
        }

        /// <summary>
        /// Private Method used to create the LogMessage for the Logger to use
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static LogMessage CreateMessage(object input)
        {
            var message = new LogMessage();
            message.Timestamp = DateTime.UtcNow.ToString();

            switch (input){
                case Exception e:
                    break;
                case string s:
                    break;
                default:
                    throw new ArgumentException("Object is not valid for Logging");
            }

        }

    }
}
