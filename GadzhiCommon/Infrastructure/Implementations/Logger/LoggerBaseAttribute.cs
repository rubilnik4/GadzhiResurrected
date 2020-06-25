using System;
using System.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Надстройка для журнала системных сообщений
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Assembly | AttributeTargets.Module)]
    public abstract class LoggerBaseAttribute : Attribute
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService LoggerService = LoggerFactory.GetFileLogger;

        protected LoggerBaseAttribute(LoggerInfoLevel loggerInfoLevel)
        {

        }

        public virtual void Init(object instance, MethodBase method, object[] args)
        {

        }

        public virtual void OnEntry()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void OnException(Exception exception)
        {

        }
    }
}