using System;
using GadzhiCommonServer.Infrastructure.Implementations;
using System.IO;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiWcfHost.Helpers
{
    /// <summary>
    /// Информация о файлах на сервере
    /// </summary>
    public static class HostSystemInformation
    {
        /// <summary>
        /// Получить путь к базе данных
        /// </summary>
        public static string DataBasePath =>
            System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath.
                Map(hostPath => new DirectoryInfo(hostPath)).
                Map(directoryPath => directoryPath?.FullName +
                                     SettingsServer.DataBaseDirectoryDefault + "\\" +
                                     SettingsServer.DataBaseNameDefault);

    }
}