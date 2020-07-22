using System;
using AspectInjector.Broker;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Атрибут для трассирования методов
    /// </summary>
    [Injection(typeof(LoggerAspect))]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
    public class LoggerAttribute : Attribute
    {
        public LoggerAttribute()
            :this (String.Empty)
        { }

        public LoggerAttribute(string message)
        {
            Message = message ?? String.Empty;
        }
        
        /// <summary>
        /// Сообщение при инициализации метода
        /// </summary>
        public string Message { get; }
    }
}