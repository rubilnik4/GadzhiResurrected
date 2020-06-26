using System;
using System.Reflection;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using MethodDecorator.Fody.Interfaces;

namespace GadzhiModules.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Атрибут для трассирования методов
    /// </summary>
    public class LoggerModulesAttribute : LoggerBaseAttribute, IMethodDecorator, IMethodInterceptor
    {
        /// <summary>
        /// Инициализация метода
        /// </summary>
        public override void Init(object instance, MethodBase method, object[] args) => base.Init(instance, method, args);

        /// <summary>
        /// Действие при входе в метод
        /// </summary>
        public override void OnEntry() => base.OnEntry();

        /// <summary>
        /// Действие при выходе из метода
        /// </summary>
        public override void OnExit() => base.OnExit();

        /// <summary>
        /// Действие при ошибке в методе
        /// </summary>
        public override void OnException(Exception exception) => base.OnException(exception);
    }
}