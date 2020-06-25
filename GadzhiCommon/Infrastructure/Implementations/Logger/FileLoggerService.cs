using System;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using NLog;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Журнал системных сообщений
    /// </summary>
    public class FileLoggerService : ILoggerService
    {
        /// <summary>
        /// Конфигурация для записи в файл информационных сообщений
        /// </summary>
        public const string LOG_TO_FILE_INFO_CONFIGURATION = "FileInfoLogger";

        /// <summary>
        /// Конфигурация для записи в файл сообщений трассировки
        /// </summary>
        public const string LOG_TO_FILE_TRACE_CONFIGURATION = "FileTraceLogger";

        /// <summary>
        /// Журнал системных сообщений типа NLog для информационных сообщений
        /// </summary>
        private readonly NLog.Logger _loggerInfo;

        /// <summary>
        /// Журнал системных сообщений типа NLog для трассировки
        /// </summary>
        private readonly NLog.Logger _loggerTrace;

        public FileLoggerService()
        {
            _loggerInfo = LogManager.GetLogger(LOG_TO_FILE_INFO_CONFIGURATION);
            _loggerTrace = LogManager.GetLogger(LOG_TO_FILE_TRACE_CONFIGURATION);
        }

        /// <summary>
        /// Сообщение уровня трассировки
        /// </summary>
        public void TraceLog(string message) => _loggerTrace.Trace(message);

        /// <summary>
        /// Сообщение уровня отладки
        /// </summary>
        public void DebugLog(string message) => _loggerInfo.Debug(message);

        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        public void InfoLog(string message) => _loggerInfo.Info(message);

        /// <summary>
        /// Сообщение предупреждающего уровня
        /// </summary>
        public void WarnLog(string message) => _loggerInfo.Warn(message);

        /// <summary>
        /// Сообщение уровня ошибки
        /// </summary>
        public void ErrorLog(Exception exception, string message) => _loggerInfo.Error(exception, message);

        /// <summary>
        /// Сообщение уровня ошибки для трассировки
        /// </summary>
        public void ErrorTraceLog(Exception exception, string message) => _loggerTrace.Error(exception, message);

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        public void FatalLog(string message) => _loggerInfo.Fatal(message);
    }
}