using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Interfaces;
using System;
using System.Linq;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Interfaces;
using static GadzhiModules.Helpers.Converters.StatusProcessingProjectConverter;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для получения информации о текущем статусе конвертирования
    /// </summary>
    public class StatusProcessingInformation : IStatusProcessingInformation
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        public IPackageData PackageInfoProject { get; }

        public StatusProcessingInformation(IPackageData packageInfoProject)
        {
            PackageInfoProject = packageInfoProject;
        }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary>
        public bool IsConverting => StatusProcessingProjectConverter.StatusProcessingProjectIsConverting.
                                    Contains(PackageInfoProject.StatusProcessingProject);

        private FilesQueueInfo FilesQueueInfo => PackageInfoProject.FilesQueueInfo;

        /// <summary>
        /// Изменился ли статус запуска/остановки процесса конвертирования
        /// </summary>
        public bool IsConvertingChanged => PackageInfoProject.StatusProcessingProject == StatusProcessingProject.Sending ||
                                           PackageInfoProject.StatusProcessingProject == StatusProcessingProject.End ||
                                           PackageInfoProject.StatusProcessingProject == StatusProcessingProject.Error;

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject => PackageInfoProject.StatusProcessingProject;

        /// <summary>
        /// Процент выполнения пакета конвертирования
        /// </summary>
        public int PercentageOfComplete => CalculatePercentageOfComplete();

        /// <summary>
        /// Есть ли у текущего процесса процент выполнения
        /// </summary>
        public bool HasStatusProcessPercentage => PackageInfoProject.StatusProcessingProject == StatusProcessingProject.InQueue ||
                                                  PackageInfoProject.StatusProcessingProject == StatusProcessingProject.Converting;

        /// <summary>
        /// Вычислить процент отконвертированных файлов
        /// </summary>
        private int CalculatePercentageOfComplete() =>
            PackageInfoProject.StatusProcessingProject switch
            {
                StatusProcessingProject.InQueue => CalculateInQueuePercentage(),
                StatusProcessingProject.Converting => CalculateConvertingPercentage(),
                _ => 0,
            };

        /// <summary>
        /// Получить статус обработки проекта c процентом выполнения
        /// </summary>       
        public string GetStatusProcessingProjectName()
        {
            string statusProcessingProjectName = StatusProcessingProjectToString(PackageInfoProject.StatusProcessingProject);

            return PackageInfoProject.StatusProcessingProject switch
            {
                StatusProcessingProject.InQueue => $"{statusProcessingProjectName}. Впереди {FilesQueueInfo.FilesInQueueCount} файлика(ов)",
                StatusProcessingProject.Converting => $"{statusProcessingProjectName}. Выполнено {PercentageOfComplete}%",
                _ => statusProcessingProjectName,
            };
        }

        /// <summary>
        /// Вычислить процент отконвертированных файлов для очереди
        /// </summary>
        private int CalculateInQueuePercentage()
        {
            if (FilesQueueInfo.FilesInQueueCount == 0 ||
                FilesQueueInfo.FirstFilesCountInQueue == 0) return 0;

            double percentagePerform = (FilesQueueInfo.FilesInQueueCount / (double)FilesQueueInfo.FirstFilesCountInQueue) * 100;
            return Convert.ToInt32(100 - percentagePerform);
        }

        /// <summary>
        /// Вычислить процент отконвертированных файлов для текущего пакета
        /// </summary>
        private int CalculateConvertingPercentage()
        {
            double numberComplete = PackageInfoProject.FilesData.Count(file => file.StatusProcessing == StatusProcessing.ConvertingComplete);
            double numberTotal = PackageInfoProject.FilesData.Count;

            double percentagePerform = (numberTotal > 0) ?
                                       (numberComplete / numberTotal) * 100 :
                                        0;
            return Convert.ToInt32(percentagePerform);
        }
    }
}
