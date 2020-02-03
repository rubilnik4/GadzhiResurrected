using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Сбросить информацию о статусе конвертирования на начальные позиции
        /// </summary>
        public void ClearFilesDataToInitialValues()
        {
            IsConverting = false;
            IsConvertingChanged = true;
            FilesQueueInfo = null;
            IsStatusProjectChanged = true;

            FilesInfoProject.ChangeAllFilesStatusAndMarkError();
        }

        /// <summary>
        /// Заполнить параметры исходя из информации о изменениях на сервере
        /// </summary>
        public void ChangeFilesDataByStatus(FilesStatus filesStatus)
        {
            switch (filesStatus.StatusProcessingProject)
            {
                case StatusProcessingProject.Sending:
                    IsConverting = true;
                    IsConvertingChanged = true;
                    break;
                case StatusProcessingProject.End:
                    IsConverting = false;
                    IsConvertingChanged = true;
                    break;
            }

            FilesQueueInfo = filesStatus.FilesQueueInfo;            
            IsStatusProjectChanged = FilesInfoProject.StatusProcessingProject != filesStatus.StatusProcessingProject;
            PercentageOfComplete = CalculatePercentageOfComplete();

            FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatus);
        }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        public bool IsConverting { get; private set; }

        /// <summary>
        /// Изменился ли статус запуска/остановки процесса конвертирования
        /// </summary>
        public bool IsConvertingChanged { get; private set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject => FilesInfoProject.StatusProcessingProject;

        /// <summary>
        /// Изменился ли статус выполнения пакета конвертирования
        /// </summary>
        public bool IsStatusProjectChanged { get; private set; }

        /// <summary>
        /// Изменился ли статус обработки проекта c процентом выполнения
        /// </summary>
        public bool IsStatusProcessingProjectNameChanged => IsStatusProjectChanged || HasStatusProcessPercentage;

        /// <summary>
        /// Информация о количестве файлов в очереди на сервере
        /// </summary>
        private FilesQueueInfo FilesQueueInfo { get; set; }

        /// <summary>
        /// Процент выполнения пакета конвертирования
        /// </summary>
        public int PercentageOfComplete { get; private set; }

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
                percentageOfComplete = 0;
            }
            else if (FilesInfoProject?.StatusProcessingProject == StatusProcessingProject.Converting)
            {
                double numberOfComplete = FilesInfoProject.
                                          FilesInfo.Count(file => file.StatusProcessing == StatusProcessing.Completed ||
                                                                  file.StatusProcessing == StatusProcessing.Error);
                double numberTotal = FilesInfoProject.
                                     FilesInfo.Count();

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
                statusProcessingProjectName += $". До вашей очереди {FilesQueueInfo.FilesInQueueCount} файл(ов)";
            }
            else if (FilesInfoProject.StatusProcessingProject == StatusProcessingProject.Converting)
            {
                statusProcessingProjectName += $". Выполнено {PercentageOfComplete}%";
            }

            return statusProcessingProjectName;
        }
    }
}
