using System;
using System.Reflection;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using MethodDecorator.Fody.Interfaces;

namespace GadzhiResurrected.Infrastructure.Implementations.Logger
{
    // Использование fody предполагает наличие класса-атрибута в библиотеке использования. 
    // Для этой цели используется наследование от абстрактного класса. Виртуальные методы сворачивать нельзя

    /// <summary>
    /// Атрибут для трассирования методов
    /// </summary>
    public class LoggerMainAttribute : LoggerBaseAttribute, IMethodDecorator
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