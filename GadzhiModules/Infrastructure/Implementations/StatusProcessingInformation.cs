using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Linq;

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
        public IFilesData FilesInfoProject { get; }

        public StatusProcessingInformation(IFilesData filesInfoProject)
        {
            FilesInfoProject = filesInfoProject;
        }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        public bool IsConverting => StatusProcessingProjectConverter.ConvertingStatusProcessingProject.
                                    Contains(FilesInfoProject.StatusProcessingProject);

        private FilesQueueInfo FilesQueueInfo => FilesInfoProject.FilesQueueInfo;

        /// <summary>
        /// Изменился ли статус запуска/остановки процесса конвертирования
        /// </summary>
        public bool IsConvertingChanged => FilesInfoProject.StatusProcessingProject == StatusProcessingProject.Sending ||
                                           FilesInfoProject.StatusProcessingProject == StatusProcessingProject.End ||
                                           FilesInfoProject.StatusProcessingProject == StatusProcessingProject.Error;

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject => FilesInfoProject.StatusProcessingProject;

        /// <summary>
        /// Процент выполнения пакета конвертирования
        /// </summary>
        public int PercentageOfComplete => CalculatePercentageOfComplete();

        /// <summary>
        /// Есть ли у текущего процесса процент выполнения
        /// </summary>
        public bool HasStatusProcessPercentage => FilesInfoProject.StatusProcessingProject == StatusProcessingProject.InQueue ||
                                                  FilesInfoProject.StatusProcessingProject == StatusProcessingProject.Converting;

        /// <summary>
        /// Вычислить процент отконвертированных файлов
        /// </summary>
        private int CalculatePercentageOfComplete()
        {
            double percentageOfComplete = 0;

            if (FilesInfoProject.StatusProcessingProject == StatusProcessingProject.InQueue)
            {
                double percentagePerform = 0;
                if (FilesQueueInfo?.FilesInQueueCount != 0 && FilesQueueInfo?.FilesInQueueCount != 0)
                {
                    percentagePerform = ((double?)FilesQueueInfo?.FilesInQueueCount / (double)FilesQueueInfo?.FirstFilesCountInQueue) * 100 ?? 0;
                }
                percentageOfComplete = 100 - percentagePerform;
            }
            else if (FilesInfoProject?.StatusProcessingProject == StatusProcessingProject.Converting)
            {
                double numberOfComplete = FilesInfoProject.
                                          FilesInfo.Count(file => file.StatusProcessing == StatusProcessing.Completed ||
                                                                  file.StatusProcessing == StatusProcessing.Error);
                double numberTotal = FilesInfoProject.FilesInfo.Count;

                percentageOfComplete = (numberOfComplete / numberTotal) * 100;
            }

            return Convert.ToInt32(percentageOfComplete);
        }

        /// <summary>
        /// Получить статус обработки проекта c процентом выполнения
        /// </summary>       
        public string GetStatusProcessingProjectName()
        {
            string statusProcessingProjectName = StatusProcessingProjectConverter.
                                                 ConvertStatusProcessingProjectToString(FilesInfoProject.
                                                                                        StatusProcessingProject);

            if (FilesInfoProject.StatusProcessingProject == StatusProcessingProject.InQueue)
            {
                statusProcessingProjectName += $". Впереди {FilesQueueInfo?.FilesInQueueCount ?? 0} файлика(ов)";
            }
            else if (FilesInfoProject.StatusProcessingProject == StatusProcessingProject.Converting)
            {
                statusProcessingProjectName += $". Выполнено {PercentageOfComplete}%";
            }

            return statusProcessingProjectName;
        }
    }
}
