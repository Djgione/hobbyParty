using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Services.Logging;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Constants = Services.Enums.Constants;

namespace UnitTests
{
    [TestClass]
    public class LoggingTest
    {
        /// <summary>
        /// This method tests if the flat file will be created if there are none that currently exist with the same fileName
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateFlatFile()
        {
            DeleteFlatFiles();
            var logger = new FlatFileLogger();
            await logger.LogAsync(new LogMessage(DateTime.UtcNow.ToString(Constants.DateStringPattern), Services.Enums.LogCategories.DEBUG, "ligma sacc"));
            var currentDay = DateTime.UtcNow.ToString(Constants.DateStringPattern).Split(' ')[0];
            var filePath = Environment.GetEnvironmentVariable("logPath", EnvironmentVariableTarget.Machine) + currentDay + ".csv";
            Assert.IsTrue(File.Exists(filePath));
            DeleteFlatFiles();
        }

        [TestMethod]
        public async Task ManyThreadsUsingFlatFileLogging()
        {
            DeleteFlatFiles();
            var loggerList = new List<FlatFileLogger>() {
                new FlatFileLogger(),
                new FlatFileLogger(),
                new FlatFileLogger()
            };
            var message = new LogMessage(DateTime.UtcNow.ToString(Constants.DateStringPattern), Services.Enums.LogCategories.DEBUG, "ligma sacc");
            try {
                var taskList = loggerList.Select(log => log.LogAsync(message));
                var results = await Task.WhenAll(taskList);
                var totalResult = true;
                foreach(bool result in results)
                {
                    if(result == false)
                    {
                        totalResult = false;
                    }
                }
                Assert.IsTrue(totalResult);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                DeleteFlatFiles();
            }
    }

        /// <summary>
        /// This method tests if the flat file will be appended to if it already exists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AppendToFlatFile()
        {
            DeleteFlatFiles();
            var logger = new FlatFileLogger();
            await logger.LogAsync(new LogMessage(DateTime.UtcNow.ToString(Constants.DateStringPattern), Services.Enums.LogCategories.DEBUG, "ligma sacc"));
            var currentDay = DateTime.UtcNow.ToString(Constants.DateStringPattern).Split(' ')[0];
            var filePath = Environment.GetEnvironmentVariable("logPath", EnvironmentVariableTarget.Machine) + currentDay + ".csv";
            var linesAfterFirstWrite = File.ReadLines(filePath).Count();
            await logger.LogAsync(new LogMessage(DateTime.UtcNow.ToString(Constants.DateStringPattern), Services.Enums.LogCategories.DEBUG, "ligma sacc part 2"));
            var linesAfterSecondWrite = File.ReadLines(filePath).Count();
            Assert.AreNotEqual(linesAfterFirstWrite, linesAfterSecondWrite);
            DeleteFlatFiles();
        }
      


        /// <summary>
        /// This Method tests to see if an exception is thrown when attempting to log some dumb shit
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Object is not valid for Logging")]
        public async Task LogMessageCreationException()
        {
            await Logger.Log(new FlatFileLogger());
        }




        private void DeleteFlatFiles()
        {
            //collects the current day from dateTime.UtcNow for finding the correct logging file
            var currentDay = DateTime.UtcNow.ToString(Constants.DateStringPattern).Split(' ')[0];
            var filePath = Environment.GetEnvironmentVariable("logPath", EnvironmentVariableTarget.Machine) + currentDay + ".csv";
            File.Delete(filePath);
        }
    }
}
