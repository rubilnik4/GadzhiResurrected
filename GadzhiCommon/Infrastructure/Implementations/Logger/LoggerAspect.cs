using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectInjector.Broker;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Аспект/триггер логгирования
    /// </summary>
    [Aspect(Scope.Global)]
    public class LoggerAspect
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService LoggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Уровень логгирования длс атрибутов
        /// </summary>
        private static LoggerLevel LoggerDefaultLevel => LoggerLevel.Debug;

        /// <summary>
        /// Действия при вызове метода или конструктора
        /// </summary>
        [Advice(Kind.Before, Targets = Target.Method | Target.Constructor)]
        public void LogEnterMethod([Argument(Source.Metadata)] MethodBase methodBase, [Argument(Source.Triggers)] Attribute[] triggers) =>
            GetMessageFromTriggers(triggers).
            WhereOk(message => !String.IsNullOrWhiteSpace(message),
                okFunc: message => message.
                                   Void(_ => LoggerService.LogByLevel(LoggerDefaultLevel, message))).
            Void(_ => LoggerService.LogByMethodBase(LoggerDefaultLevel, methodBase));

        /// <summary>
        /// Действия при установке свойства
        /// </summary>
        [Advice(Kind.Before, Targets = Target.Setter)]
        public void LogEnteSetProperty([Argument(Source.Metadata)] MethodBase methodBase, [Argument(Source.Arguments)] object[] arguments) =>
            LoggerService.LogByPropertySet(LoggerDefaultLevel, methodBase, arguments?.FirstOrDefault()?.ToString());


        /// <summary>
        /// Получить сообщение из атрибутов
        /// </summary>
        private static string GetMessageFromTriggers(IEnumerable<Attribute> triggers) =>
            triggers?.
            OfType<LoggerAttribute>().
            FirstOrDefault()?.Message
            ?? String.Empty;
    }
}