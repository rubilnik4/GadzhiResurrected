using System;
using System.Collections.Generic;
using System.Reflection;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Infrastructure.Interfaces.Logger
{
    /// <summary>
    /// Журнал системных сообщений
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Сообщение уровня трассировки
        /// </summary>
        void TraceLog(string message);

        /// <summary>
        /// Сообщение уровня отладки
        /// </summary>
        void DebugLog(string message);

        /// <summary>
        /// Сообщение уровня отладки
        /// </summary>
        void DebugLog(string message, IEnumerable<string> parameters);

        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        void InfoLog(string message);

        /// <summary>
        /// Сообщение информационного уровня
        /// </summary>
        public void InfoLog(string message, IEnumerable<string> parameters);

        /// <summary>
        /// Сообщение предупреждающего уровня
        /// </summary>
        void WarnLog(string message);

        /// <summary>
        /// Сообщение уровня ошибки
        /// </summary>
        void ErrorLog(Exception exception, string message);

        /// <summary>
        /// Сообщения уровня ошибки
        /// </summary>
        void ErrorLog(IErrorCommon fileError);

        /// <summary>
        /// Сообщения уровня ошибки
        /// </summary>
        void ErrorsLog(IEnumerable<IErrorCommon> fileErrors);

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        void FatalLog(string message);

        /// <summary>
        /// Вывести сообщение согласно уровня
        /// </summary>
        void LogByLevel(LoggerLevel loggerLevel, string message);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        void LogByObject<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction, MethodBase methodBase);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        void LogByObject(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction, MethodBase methodBase, string parameter);

        /// <summary>
        /// Вывести сообщение об объекте согласно типа действия
        /// </summary>
        void LogByObject<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction,
                                 MethodBase methodBase, TValue parameter, string parentValue);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        void LogByObjects<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction,
                                 MethodBase methodBase, IEnumerable<TValue> parameters);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        void LogByObjects<TValue>(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction, MethodBase methodBase,
                                 IEnumerable<TValue> parameters, string parentValue);

        /// <summary>
        /// Вывести сообщение об объектах согласно типа действия
        /// </summary>
        void LogByObjects(LoggerLevel loggerLevel, LoggerObjectAction loggerObjectAction, string objectName,
                         MethodBase methodBase, IEnumerable<string> parameters, string parentValue);

        /// <summary>
        /// Вывести сообщение согласно информации о методе или свойстве
        /// </summary>
        void LogByMethodBase(LoggerLevel loggerLevel, MethodBase methodBase);

        /// <summary>
        /// Вывести сообщение согласно информации о новом значении свойства
        /// </summary>
        void LogByPropertySet(LoggerLevel loggerLevel, MethodBase propertyBase, string newValue);
    }
}
