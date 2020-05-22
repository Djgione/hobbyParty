using Services.Enums;
using Services.Interfaces;
using Services.Logging;
using Services.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        
        /// <summary>
        /// This method accepts an object to write to log, and then performs a write operation for each logger stored within the static enumeration
        /// </summary>
        /// <param name="o">Accepts any Object o, but really will only work with string or exception</param>
        /// <returns>A bool determining that all loggers succeeded</returns>
        public static async Task<bool> Log(object o)
        {
            try
            {
                var message = CreateMessage(o);
                //Makes a list of Task<bool>'s for each logger in _loggers by using their LogAsync(LogMessage message) method, then awaits them
                var taskList = _loggers.Select(logger => logger.LogAsync(message));
                await Task.WhenAll(taskList);

                //If any of the tasks returned false, method returns false
                foreach(Task<bool> task in taskList)
                {
                    if (task.Result == false)
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Private Method used to create the LogMessage for the Logger to use
        /// </summary>
        /// <param name="input">Whatever object is passed into the Log, whether it be an exception or a string</param>
        /// <returns></returns>
        private static LogMessage CreateMessage(object input)
        {
            var message = new LogMessage
            {
                Timestamp = DateTime.UtcNow.ToString(Constants.DateStringPattern)
            };

            switch (input){
                case Exception e:
                    message.Message = e.ToString();
                    message.Category = LogCategories.ERROR;
                    break;
                case string s:
                    message.Message = s;
                    message.Category = LogCategories.DEBUG;
                    break;
                default:
                    throw new ArgumentException("Object is not valid for Logging");
            }

            return message;
        }

    }
}
