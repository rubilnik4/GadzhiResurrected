using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
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
        /// Сообщение уровня ошибки для трассировки
        /// </summary>
        public void ErrorTraceLog(Exception exception, string message) => _logger.Error(exception, message);

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        public void FatalLog(string message) => _logger.Fatal(message);

        /// <summary>
        /// Вывести сообщение согласно информации о методе
        /// </summary>
        public void LogByMethodBase(LoggerInfoLevel loggerInfoLevel, MethodBase methodBase) =>
            LogByLevel(loggerInfoLevel, $"Enter {GetReflectionName(methodBase)}");

        /// <summary>
        /// Вывести сообщение согласно информации о новом значении свойства
        /// </summary>
        public void LogByPropertySet(LoggerInfoLevel loggerInfoLevel, MethodBase propertyBase, string newValue) =>
            LogByLevel(loggerInfoLevel, $"Set {GetReflectionName(MemberTypes.Property, propertyBase)}: {newValue ?? "NullValue"}");

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

        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        private static string GetReflectionName(MemberTypes memberType, MethodBase method) =>
            (method != null)
            ? GetReflectionName(memberType,
                                    method.ReflectedType?.Name ?? String.Empty,
                                    method.Name)
            : throw new ArgumentNullException(nameof(method));

        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        private static string GetReflectionName(MethodBase method) =>
            (method != null)
            ? GetReflectionName(method.MemberType,
                                method.ReflectedType?.Name ?? String.Empty,
                                method.Name)
            : throw new ArgumentNullException(nameof(method));

        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        private static string GetReflectionName(MemberTypes memberType, string className, string methodName) =>
            memberType switch
            {
                MemberTypes.Constructor => GetMethodInfo(memberType, className),
                MemberTypes.Method => GetMethodInfo(memberType, className, methodName),
                MemberTypes.Property => GetMethodInfo(memberType, className, methodName),
                _ => GetMethodInfo(memberType, className, methodName),
            };

        /// <summary>
        /// Представить информацию о методе в строковом значении
        /// </summary>
        private static string GetMethodInfo(MemberTypes memberType, string className, string methodName) =>
            GetMethodInfo(memberType, className) + $".{methodName}";

        /// <summary>
        /// Представить информацию о методе в строковом значении
        /// </summary>
        private static string GetMethodInfo(MemberTypes memberType, string className) =>
            $"[{memberType}]{className}";

        /// <summary>
        /// Преобразовать список параметров в строку с отступами
        /// </summary>
        private static string FormatMessageParameters(string message, IEnumerable<string> parameters) =>
            message + "\n\t- " + String.Join("\n\t- ", parameters);
    }
}