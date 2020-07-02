using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.Errors;
using NLog;
using static GadzhiCommon.Infrastructure.Implementations.Logger.LoggerFormatMessages;

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
        /// Сообщение уровня отладки
        /// </summary>
        public void DebugLog(string message, IEnumerable<string> parameters) =>
            _logger.Debug(FormatMessageParameters(message, parameters));

        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        public void InfoLog(string message) => _logger.Info(message);

        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        public void InfoLog(string message, IEnumerable<string> parameters) =>
            _logger.Info(FormatMessageParameters(message, parameters));

        /// <summary>
        /// Сообщение предупреждающего уровня
        /// </summary>
        public void WarnLog(string message) => _logger.Warn(message);

        /// <summary>
        /// Сообщение уровня ошибки
        /// </summary>
        public void ErrorLog(Exception exception, string message) => _logger.Error(exception, message);

        /// <summary>
        /// Сообщения уровня ошибки
        /// </summary>
        public void ErrorLog(IErrorCommon fileError)
        {
            if (fileError == null) return;

            if (fileError.Exception != null)
            {
                _logger.Error(fileError.Exception, fileError.ErrorType.ToString);
            }
            else
            {
                _logger.Error($"{fileError.ErrorType}: {fileError.Description}");
            }
        }

        /// <summary>
        /// Сообщения уровня ошибки
        /// </summary>
        public void ErrorsLog(IEnumerable<IErrorCommon> fileErrors)
        {
            foreach (var error in fileErrors ?? Enumerable.Empty<IErrorCommon>())
            {
                ErrorLog(error);
            }
        }

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        public void FatalLog(string message) => _logger.Fatal(message);

        /// <summary>
        /// Вывести сообщение согласно информации о методе
        /// </summary>
        public void LogByMethodBase(LoggerLevel loggerLevel, MethodBase methodBase) =>
            LogByLevel(loggerLevel, $"Enter {GetReflectionName(methodBase)}");

        /// <summary>
        /// Вывести сообщение согласно информации о новом значении свойства
        /// </summary>
        public void LogByPropertySet(LoggerLevel loggerLevel, MethodBase propertyBase, string newValue) =>
            LogByLevel(loggerLevel, $"Set {GetReflectionName(MemberTypes.Property, propertyBase)}: {newValue ?? "NullValue"}");

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        public void LogByObject<TValue>(LoggerLevel loggerLevel, LoggerAction loggerAction, MethodBase methodBase) =>
            LogByObject(loggerLevel, loggerAction, methodBase, Enumerable.Empty<TValue>(), String.Empty);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        public void LogByObject(LoggerLevel loggerLevel, LoggerAction loggerAction, MethodBase methodBase, string parameter) =>
            LogByObject(loggerLevel, loggerAction, methodBase, parameter, String.Empty);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObject(LoggerLevel loggerLevel, LoggerAction loggerAction, string objectName, MethodBase methodBase, string parameters) =>
            LogByObjects(loggerLevel, loggerAction, objectName, methodBase, new List<string>() { parameters }, String.Empty);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        public void LogByObject<TValue>(LoggerLevel loggerLevel, LoggerAction loggerAction,
                                        MethodBase methodBase, TValue parameter, string parentValue) =>
            LogByObjects(loggerLevel, loggerAction, methodBase, new List<TValue>() { parameter }, parentValue);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия метода
        /// </summary>
        public void LogByObjectMethod(LoggerLevel loggerLevel, LoggerAction loggerAction, MethodBase methodBase, string parameter) =>
            LogByObjects(loggerLevel, loggerAction, ReflectionInfo.GetTypeNameFromMethodBase(methodBase), methodBase,
                         new List<string>() { parameter }, String.Empty);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObjects<TValue>(LoggerLevel loggerLevel, LoggerAction loggerAction, MethodBase methodBase, IEnumerable<TValue> parameters) =>
            LogByObjects(loggerLevel, loggerAction, methodBase, parameters, String.Empty);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObjects<TValue>(LoggerLevel loggerLevel, LoggerAction loggerAction, MethodBase methodBase,
                                         IEnumerable<TValue> parameters, string parentValue) =>
            LogByObjects(loggerLevel, loggerAction, ReflectionInfo.GetTypeName(typeof(TValue)), methodBase,
                        parameters?.Select(parameter => parameter.ToString()), parentValue);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObjects(LoggerLevel loggerLevel, LoggerAction loggerAction, string typeName, 
                                 MethodBase methodBase, IEnumerable<string> parameters, string parentValue) =>
            LogByLevel(loggerLevel, $"{loggerAction} {FormatTypeName(typeName)} by {GetReflectionName(methodBase)}{GetParentClassMessage(parentValue)}".
                                    Map(message => FormatMessageParameters(message, parameters)));
        /// <summary>
        /// Вывести сообщение согласно уровня
        /// </summary>
        public void LogByLevel(LoggerLevel loggerLevel, string message)
        {
            if (String.IsNullOrWhiteSpace(message)) return;

            switch (loggerLevel)
            {
                case LoggerLevel.Trace:
                    TraceLog(message);
                    break;
                case LoggerLevel.Debug:
                    DebugLog(message);
                    break;
                case LoggerLevel.Info:
                    InfoLog(message);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(loggerLevel), (int)loggerLevel, typeof(LoggerLevel));
            }
        }

 
    }
}