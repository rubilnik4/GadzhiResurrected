using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GadzhiCommon.Infrastructure.Implementations.Reflection
{
    /// <summary>
    /// Информация об исполняемом коде
    /// </summary>
    public static class ReflectionInfo
    {
        /// <summary>
        /// Получить информацию о методе
        /// </summary>
        public static MethodInfo GetMethodBase<TValue>(TValue caller, [CallerMemberName] string methodName = "")
            where TValue : class =>
            caller.GetType().
            GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).
            FirstOrDefault(method => method.Name == methodName);
    }
}