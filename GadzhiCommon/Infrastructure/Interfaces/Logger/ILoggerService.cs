﻿using System;
using GadzhiCommon.Models.Enums;

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
        /// Сообщение информационного уровня
        /// </summary>
        void InfoLog(string message);

        /// <summary>
        /// Сообщение предупреждающего уровня
        /// </summary>
        void WarnLog(string message);

        /// <summary>
        /// Сообщение уровня ошибки
        /// </summary>
        void ErrorLog(Exception exception, string message);

        /// <summary>
        /// Сообщение уровня ошибки для трассировки
        /// </summary>
        void ErrorTraceLog(Exception exception, string message);

        /// <summary>
        /// Сообщение критического уровня
        /// </summary>
        void FatalLog(string message);

        /// <summary>
        /// Вывести сообщение согласно уровня
        /// </summary>
        void LogByLevel(LoggerInfoLevel loggerInfoLevel, string message);
    }
}