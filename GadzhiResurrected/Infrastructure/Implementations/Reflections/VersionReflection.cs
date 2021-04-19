using System.Deployment.Application;

namespace GadzhiResurrected.Infrastructure.Implementations.Reflections
{
    /// <summary>
    /// Параметры сборки
    /// </summary>
    public static class VersionReflection
    {
        /// <summary>
        /// Получить версию программы
        /// </summary>
        public static string GetClickOnceVersion() =>
            ApplicationDeployment.IsNetworkDeployed
                ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                : "Debug";
    }
}