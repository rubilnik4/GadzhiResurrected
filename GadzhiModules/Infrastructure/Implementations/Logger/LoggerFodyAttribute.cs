using System;
using System.Reflection;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Enums;
using MethodDecorator.Fody.Interfaces;

namespace GadzhiModules.Infrastructure.Implementations.Logger
{
    public class LoggerAttribute: LoggerBaseAttribute, IMethodDecorator
    {
        public LoggerAttribute(LoggerInfoLevel loggerInfoLevel)
            :base(loggerInfoLevel)
        {

        }

        public override void Init(object instance, MethodBase method, object[] args)
        {
            base.Init(instance, method, args);
        }

        public override void OnEntry()
        {
            base.OnEntry();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnException(Exception exception)
        {
            base.OnException(exception);
        }
    }
}