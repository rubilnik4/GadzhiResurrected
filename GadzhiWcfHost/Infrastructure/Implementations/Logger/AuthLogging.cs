using System;
using GadzhiWcfHost.Infrastructure.Implementations.Client;

namespace GadzhiWcfHost.Infrastructure.Implementations.Logger
{
    /// <summary>
    /// Параметры логгирования с аутентификацией
    /// </summary>
    public static class AuthLogging
    {
        /// <summary>
        /// Добавить к параметру значение аутентификации
        /// </summary>
        public static string GetParameterAuth<TValue>(TValue parameter) =>
            $"[{Authentication.Authentication.GetIdentityName()}] {parameter?.ToString() ?? String.Empty}";
    }
}