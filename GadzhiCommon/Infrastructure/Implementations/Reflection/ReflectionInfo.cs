using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using GadzhiCommon.Extensions.Functional;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace GadzhiCommon.Infrastructure.Implementations.Reflection
{
    /// <summary>
    /// Информация об исполняемом коде
    /// </summary>
    public static class ReflectionInfo
    {
        /// <summary>
        /// Получить информацию о методе с атрибутом
        /// </summary>
        public static MethodInfo GetMethodBase<TValue>(TValue caller, [CallerMemberName] string methodName = "")
            where TValue : class =>
            GetMethodBaseDefinition(caller, methodName);

        /// <summary>
        /// Получить возвращаемый тип метода
        /// </summary>
        public static string GetTypeNameFromMethod<TValue>(TValue caller, [CallerMemberName] string methodName = "")
            where TValue : class =>
            GetMethodBaseDefinition(caller, methodName).
            Map(methodBase => GetTypeName(methodBase?.ReturnType));

        /// <summary>
        /// Получить возвращаемый тип метода
        /// </summary>
        public static string GetTypeNameFromMethodBase(MethodBase method) =>
            (method is MethodInfo memberInfo)
                ? GetTypeName(memberInfo.ReturnType)
                : String.Empty;

        /// <summary>
        /// Преобразовать коллекцию типов в строку
        /// </summary>
        public static string GetTypeNames(IList<Type> types) =>
            types?.
            Select(GetTypeName).
            Map(typeNames => String.Join(",", typeNames))
            ?? throw new ArgumentNullException(nameof(types));

        /// <summary>
        /// Получить имя типа
        /// </summary>
        public static string GetTypeName(Type type) =>
            type switch
            {
                null => throw new ArgumentNullException(nameof(type)),
                _ when type.IsGenericType => $"{FormatGenericType(type)}<{GetTypeNames(type.GetGenericArguments())}>",
                _ => type.Name,
            };

        /// <summary>
        /// Получить имя метода в выражении
        /// </summary>
        public static string GetExpressionName<T>(Expression<T> expression) =>
            ((MethodCallExpression)expression?.Body)?.Method.Name 
            ?? throw new ArgumentNullException(nameof(expression));

        /// <summary>
        /// Получить информацию о методе
        /// </summary>
        private static MethodInfo GetMethodBaseDefinition<TValue>(TValue caller, string methodName)
           where TValue : class =>
           caller.GetType().
           GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).
           FirstOrDefault(method => method.Name == methodName);

        /// <summary>
        /// Удалить артефакты из имени с общим типом
        /// </summary>
        private static string FormatGenericType(Type genericType) =>
            genericType?.Name.Remove(genericType.Name.IndexOf('`'))
            ?? throw new ArgumentNullException(nameof(genericType));
    }
}