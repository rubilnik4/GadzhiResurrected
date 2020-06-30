using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.Errors;
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
        public void LogByObject<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction, MethodBase methodBase) =>
            LogByObject(loggerLevel, loggerObjectAction, methodBase, Enumerable.Empty<TValue>(), String.Empty);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        public void LogByObject(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction, MethodBase methodBase, string parameter) =>
            LogByObject(loggerLevel, loggerObjectAction, methodBase, new List<string>() { parameter }, String.Empty);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        public void LogByObject<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction,
                                        MethodBase methodBase, TValue parameter, string parentValue) =>
            LogByObjects(loggerLevel, loggerObjectAction, methodBase, new List<TValue>() { parameter }, parentValue);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObjects<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction,
                                         MethodBase methodBase, IEnumerable<TValue> parameters) =>
            LogByObjects(loggerLevel, loggerObjectAction, methodBase, parameters, String.Empty);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObjects<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction,
                                        MethodBase methodBase, IEnumerable<TValue> parameters, string parentValue) =>
            LogByObjects(loggerLevel, loggerObjectAction, typeof(TValue).Name, methodBase,
                        parameters?.Select(parameter => parameter.ToString()), parentValue);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        public void LogByObjects(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction,
                                string objectName, MethodBase methodBase, IEnumerable<string> parameters, string parentValue) =>
            LogByLevel(loggerLevel, $"{loggerObjectAction} {objectName} by {GetReflectionName(methodBase)}{GetParentClassMessage(parentValue)}".
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
        /// Преобразовать значение родительского класса в сообщение 
        /// </summary>
        private static string GetParentClassMessage(string parentValue) =>
            (!String.IsNullOrWhiteSpace(parentValue))
            ? $" in {parentValue}"
            : String.Empty;


        /// <summary>
        /// Преобразовать список параметров в строку с отступами
        /// </summary>
        private static string FormatMessageParameters(string message, IEnumerable<string> parameters) =>
            parameters?.ToList().
            Map(parametersCollection => parametersCollection switch
            {
                _ when parametersCollection?.Count > 1 => message + "\n\t- " + String.Join("\n\t- ", parametersCollection),
                _ when parametersCollection?.Count == 1 => message + ": " + parametersCollection[0],
                _ => message,
            });
    }
}