using System;
using System.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    // Использование fody предполагает наличие класса-атрибута в библиотеке использования. 
    // Для этой цели используется наследование от абстрактного класса. Виртуальные методы сворачивать нельзя

    /// <summary>
    /// Надстройка для журнала системных сообщений
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor),]
    public abstract class LoggerBaseAttribute : Attribute
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService LoggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Сообщение для записи
        /// </summary>
        public LoggerActionAttribute LoggerAction { get; set; } = LoggerActionAttribute.OnEnter;

        /// <summary>
        /// Сообщение для записи
        /// </summary>
        public string MessageEntry { get; set; } = String.Empty;

        /// <summary>
        /// Уровень логгирования
        /// </summary>
        public LoggerInfoLevel LoggerInfoLevel { get; set; } = LoggerInfoLevel.Debug;

        /// <summary>
        /// Информация о вызываемом методе
        /// </summary>
        private MethodBase _method;

        /// <summary>
        /// Инициализация метода
        /// </summary>
        public virtual void Init(object instance, MethodBase method, object[] args)
        {
            _method = method;
        }

        /// <summary>
        /// Действие при входе в метод
        /// </summary>
        public virtual void OnEntry()
        {
            if (!String.IsNullOrWhiteSpace(MessageEntry))
            {
                LoggerService.LogByLevel(LoggerInfoLevel, MessageEntry);
            }

            if (IsActionOnEnter(LoggerAction) && _method != null)
            {
                string logPrefix = LoggerAction == LoggerActionAttribute.OnEnterAndExit ? "Enter" : "Using";
                LoggerService.DebugLog($"{logPrefix}:{GetReflectionName(_method)}");
            }
        }

        /// <summary>
        /// Действие при выходе из метода
        /// </summary>
        public virtual void OnExit()
        {
            if (!IsActionOnExit(LoggerAction) || _method == null) return;
            LoggerService.DebugLog($"Exit:{GetReflectionName(_method)}");
        }

        /// <summary>
        /// Действие при ошибке в методе
        /// </summary>
        public virtual void OnException(Exception exception)
        { }

        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        private static string GetReflectionName(MethodBase method)
        {
            var memberType = method.MemberType;
            string className = method.ReflectedType?.Name ?? String.Empty;
            string methodName = method.Name;

            return memberType switch
            {
                MemberTypes.Constructor => $"[{memberType}]{className}",
                MemberTypes.Method => $"[{memberType}]{className}.{methodName}",
                MemberTypes.Property => $"[{memberType}]{className}.{methodName}",
                _ => $"[{memberType}]{className}.{methodName}",
            };
        }

        /// <summary>
        /// Выполнять ли действие при входе в метод
        /// </summary>
        private static bool IsActionOnEnter(LoggerActionAttribute loggerAction) =>
            loggerAction == LoggerActionAttribute.OnEnter || loggerAction == LoggerActionAttribute.OnEnterAndExit;

        /// <summary>
        /// Выполнять ли действие при выходе в метод
        /// </summary>
        private static bool IsActionOnExit(LoggerActionAttribute loggerAction) => loggerAction == LoggerActionAttribute.OnEnterAndExit;
    }
}