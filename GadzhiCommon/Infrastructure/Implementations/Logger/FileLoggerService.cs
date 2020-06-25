using System;
using System.ComponentModel;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Implementations.Functional;
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
        public const string FILE_LOG_CONFIGURATION = "FileLogger";

        /// <summary>
        /// Журнал системных сообщений типа NLog для информационных сообщений
        /// </summary>
        private readonly NLog.Logger _logger;

        public FileLoggerService()
        {
            _logger = LogManager.GetLogger(FILE_LOG_CONFIGURATION);
        }

        /// <summary>
        /// Сообщение уровня трассировки
        /// </summary>
        public void TraceLog(string message) => _logger.Trace(message);

        /// <summary>
        /// Сообщение уровня отладки
        /// </summary>
        public void DebugLog(string message) => _logger.Debug(message);

        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        public void InfoLog(string message) => _logger.Info(message);

        /// <summary>
        /// Сообщение предупреждающего уровня
        /// </summary>
        public void WarnLog(string message) => _logger.Warn(message);

        /// <summary>
        /// Сообщение уровня ошибки
        /// </summary>
        public void ErrorLog(Exception exception, string message) => _logger.Error(exception, message);

        /// <summary>
        /// Сообщение уровня ошибки для трассировки
        /// </summary>
        public void ErrorTraceLog(Exception exception, string message) => _logger.Error(exception, message);

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        public void FatalLog(string message) => _logger.Fatal(message);

        /// <summary>
        /// Вывести сообщение согласно уровня
        /// </summary>
        public void LogByLevel(LoggerInfoLevel loggerInfoLevel, string message)
        {
            if (String.IsNullOrWhiteSpace(message)) return;

            switch (loggerInfoLevel)
            {
                case LoggerInfoLevel.Trace:
                    TraceLog(message);
                    break;
                case LoggerInfoLevel.Debug:
                    DebugLog(message);
                    break;
                case LoggerInfoLevel.Info:
                    InfoLog(message);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(loggerInfoLevel), (int)loggerInfoLevel, typeof(LoggerInfoLevel));
            }
        }
    }
}