using System;
using System.Collections.Generic;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using Unity;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    public class ProfilingInterceptionBehavior : IInterceptionBehavior
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService;

        public ProfilingInterceptionBehavior(ILoggerService loggerService)
        {
            _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
        }
       
        /// <summary>
        /// Трассировать метод
        /// </summary>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var startTime = DateTime.Now;

            _loggerService.TraceLog($"Invoking method {input.MethodBase.Name} at {startTime.ToLongTimeString()}");
            var result = getNext()(input, getNext);

            var endTime = DateTime.Now;

            if (result.Exception == null)
            {
                _loggerService.TraceLog($"Returning method {input.MethodBase.Name} at {endTime.ToLongTimeString()}");
            }
            else
            {
                _loggerService.ErrorTraceLog(result.Exception, $"Method {input.MethodBase.Name} threw exception at {endTime.ToLongTimeString()}");
            }

            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces() => Type.EmptyTypes;

        public bool WillExecute => true;
    }
}