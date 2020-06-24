using System;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Журнал системных сообщений
    /// </summary>
    public class FileLoggerService : ILoggerService
    {
        /// <summary>
        /// Конфигурация для записи в файл
        /// </summary>
        public const string LOG_TO_FILE_CONFIGURATION = "NLogFile";

        /// <summary>
        /// Журнал системных сообщений типа NLog
        /// </summary>
        private readonly NLog.Logger _logger;

        public FileLoggerService()
        {
            LogManager.Configuration=
            _logger = LogManager.GetLogger(LOG_TO_FILE_CONFIGURATION);
        }

        /// <summary>
        /// Сообщение уровня отладки
        /// </summary>
        public void DebugLog(string message) => _logger.Debug(message);


        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        public void InfoLog(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        /// Сообщение предупреждающего уровня
        /// </summary>
        public void WarnLog(string message) => _logger.Warn(message);

        /// <summary>
        /// Сообщение уровня ошибки
        /// </summary>
        public void ErrorLog(Exception exception, string message) => _logger.Error(exception, message);

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        public void FatalLog(string message) => _logger.Fatal(message);

        public static LoggingConfiguration GetFileLoggingConfiguration()
        {
            var config = new LoggingConfiguration();
            config.LogFactory.ThrowExceptions = true;
           

            var fileLogTarget = new FileTarget("FileLog")
            {
                FileName = @".\Logs\GadzhiLog.log"
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileLogTarget);


            //< !--Логгирование-- >
            //    < nlog xmlns = "http://www.nlog-project.org/schemas/NLog.xsd"
            //xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance"
            //autoReload = "true"
            //throwExceptions = "true"
            //internalLogLevel = "Debug"
            //internalLogFile = ".\Logs\internal-nlog.log"
            //internalLogToConsole = "false" >

            //    < targets >
            //    < target name = "FileLog"
            //xsi: type = "File"
            //fileName = ".\Logs\gadzhiLog.log" />
            //    </ targets >

            //    < targets async = "true" >

            //    < target name = "NLogFile"
            //xsi: type = "File" />

            //    </ targets >


            //    < rules >

            //    < logger name = "NLogFile"
            //minlevel = "Debug"
            //writeTo = "FileLog"
            //layout = "${longdate}|${level:uppercase=true}|${logger}|${message}" />
            //    </ rules >
            //    </ nlog >

        }
    }
}