using System;
using System.Web.Services.Description;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client.Logger
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
            $"[{Authentication.GetIdentityName()}] {parameter?.ToString() ?? String.Empty}";
    }
}