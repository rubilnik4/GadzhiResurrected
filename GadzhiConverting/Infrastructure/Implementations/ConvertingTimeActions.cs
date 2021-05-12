using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Ежедневные действия предварительной загрузки
    /// </summary>
    public static class ConvertingTimeActions
    {
        /// <summary>
        /// Проверить ресурсы в базе
        /// </summary>
        [Logger]
        public static async Task SignaturesOnDataBase(IProjectSettings projectSettings, IMessagingService messagingService, 
                                                       ILoggerService loggerService)
        {
            var dateTimeNow = DateTime.Now;
            var timeElapsed = new TimeSpan((dateTimeNow - Properties.Settings.Default.SignaturesCheck).Ticks);
            if (timeElapsed.TotalHours > ProjectSettings.IntervalSignatureUpdate)
            {
                messagingService.ShowMessage("Перезагрузка ресурсов...");
                loggerService.DebugLog("Перезагрузка ресурсов...");

                await projectSettings.ConvertingResources.ReloadResources();

                Properties.Settings.Default.SignaturesCheck = new TimeSpan(dateTimeNow.Ticks);
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Проверить и удалить ненужные пакеты в базе
        /// </summary>
        [Logger]
        public static async Task CheckAndDeleteUnusedPackagesOnDataBase(ConvertingServerServiceFactory convertingServerServiceFactory,
                                                                  IMessagingService messagingService, ILoggerService loggerService)
        {
            var dateTimeNow = DateTime.Now;
            var timeElapsed = new TimeSpan((dateTimeNow - Properties.Settings.Default.UnusedDataCheck).Ticks);
            if (timeElapsed.TotalHours > ProjectSettings.IntervalHoursToDeleteUnusedPackages)
            {
                messagingService.ShowMessage("Очистка неиспользуемых пакетов...");
                loggerService.DebugLog("Очистка неиспользуемых пакетов...");

                var result = await convertingServerServiceFactory.UsingServiceRetry(service => service.Operations.DeleteAllUnusedPackagesUntilDate(dateTimeNow));
                if (result.HasErrors)
                {
                    messagingService.ShowErrors(result.Errors);
                    loggerService.ErrorsLog(result.Errors);
                }

                Properties.Settings.Default.UnusedDataCheck = new TimeSpan(dateTimeNow.Ticks);
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Очистить папку с обработанными файлами на жестком диске
        /// </summary>
        [Logger]
        public static async Task DeleteAllUnusedDataOnDisk(IFileSystemOperations fileSystemOperations,
                                                            IMessagingService messagingService, ILoggerService loggerService)
        {
            var dateTimeNow = DateTime.Now;
            var timeElapsed = new TimeSpan((dateTimeNow - Properties.Settings.Default.ConvertingDataFolderCheck).Ticks);
            if (timeElapsed.TotalHours > ProjectSettings.IntervalHoursToDeleteUnusedPackages)
            {
                messagingService.ShowMessage("Очистка пространства на жестком диске...");
                loggerService.DebugLog("Очистка пространства на жестком диске...");

                await Task.Run(() => fileSystemOperations.DeleteAllDataInDirectory(ProjectSettings.ConvertingDirectory, DateTime.Now,
                                                                                   ProjectSettings.IntervalHoursToDeleteUnusedPackages));

                Properties.Settings.Default.ConvertingDataFolderCheck = new TimeSpan(dateTimeNow.Ticks);
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Удалить все предыдущие запущенные процессы
        /// </summary>
        [Logger]
        public static void KillPreviousRunProcesses()
        {
            var processes = Process.GetProcesses().
                            Where(process => process.ProcessName.ContainsIgnoreCase("ustation") ||
                                             process.ProcessName.ContainsIgnoreCase("winword") ||
                                             process.ProcessName.ContainsIgnoreCase("excel"));
            foreach (var process in processes) process.Kill();
        }
    }
}