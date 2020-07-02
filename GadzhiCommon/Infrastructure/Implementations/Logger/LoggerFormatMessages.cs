using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiCommon.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Форматирование текстовых сообщений для логгера
    /// </summary>
    public static class LoggerFormatMessages
    {
        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        public static string GetReflectionName(MemberTypes memberType, MethodBase method) =>
            (method != null)
            ? GetReflectionName(memberType,
                                    method.ReflectedType?.Name ?? String.Empty,
                                    method.Name)
            : throw new ArgumentNullException(nameof(method));

        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        public static string GetReflectionName(MethodBase method) =>
            (method != null)
            ? GetReflectionName(method.MemberType,
                                method.ReflectedType?.Name ?? String.Empty,
                                method.Name)
            : throw new ArgumentNullException(nameof(method));

        /// <summary>
        /// Получить имя метода и его класс
        /// </summary>
        public static string GetReflectionName(MemberTypes memberType, string className, string methodName) =>
            memberType switch
            {
                MemberTypes.Constructor => GetMethodInfo(memberType, className),
                MemberTypes.Method => GetMethodInfo(memberType, className, methodName),
                MemberTypes.Property => GetMethodInfo(memberType, className, methodName),
                _ => GetMethodInfo(memberType, className, methodName),
            };

        /// <summary>
        /// Преобразовать значение родительского класса в сообщение 
        /// </summary>
        public static string GetParentClassMessage(string parentValue) =>
            (!String.IsNullOrWhiteSpace(parentValue))
            ? $" in {parentValue}"
            : String.Empty;

        /// <summary>
        /// Преобразовать список параметров в строку с отступами
        /// </summary>
        public static string FormatMessageParameters(string message, IEnumerable<string> parameters) =>
            parameters?.ToList().
            Map(parametersCollection => parametersCollection switch
            {
                _ when parametersCollection?.Count > 1 => message + "\n\t- " + String.Join("\n\t- ", parametersCollection),
                _ when parametersCollection?.Count == 1 => message + ": " + parametersCollection[0],
                _ => message,
            });

        /// <summary>
        /// Форматирование типа объекта
        /// </summary>
        public static string FormatTypeName(string typeName) =>
            !String.IsNullOrWhiteSpace(typeName)
                ? $" {typeName}"
                : String.Empty;

        /// <summary>
        /// Представить информацию о методе в строковом значении
        /// </summary>
        private static string GetMethodInfo(MemberTypes memberType, string className, string methodName) =>
            GetMethodInfo(memberType, className) + $".{methodName}";

        /// <summary>
        /// Представить информацию о методе в строковом значении
        /// </summary>
        private static string GetMethodInfo(MemberTypes memberType, string className) =>
            $"[{memberType}]{className}";
    }
}