using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logging
{
    public class FlatFileLogger:ILogger 
    {
        private readonly string _filePath = Environment.GetEnvironmentVariable("logPath", EnvironmentVariableTarget.Machine);
        private readonly string _fileType = ".csv";
        /// <summary>
        /// Attempts to Log the message into a flatFile for each day, and if one doesn't exist it creates it
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Task wrapping bool indicating success</returns>
        public async Task<bool> LogAsync(LogMessage message)
        {
            try
            {
                Directory.CreateDirectory(_filePath);

                //split message TimeStamp into yyyy.MM.dd and HH:mm:ss:ffff
                var dateSplit = message.Timestamp.Split(' ');
                var fileName = _filePath + dateSplit[0] + _fileType;

                //create builder and add the time of log, message.Category, message.Message, and a comma with a new line operator
                var builder = new StringBuilder();
                builder.Append(dateSplit[1] + "| ");
                builder.Append(message.Category + ": ");
                builder.Append(message.Message);
                builder.Append(",\n");

                //awaits process of adding all text contained within stringbuilder to the fileName, and can create in the process
                await File.AppendAllTextAsync(fileName, builder.ToString()).ConfigureAwait(false);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

    }
}
