using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Interfaces;
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
        /// Слой инфраструктуры
        /// </summary>        
        private IApplicationGadzhi ApplicationGadzhi { get; }

        public StatusProcessingInformation(IApplicationGadzhi applicationGadzhi)
        {
            ApplicationGadzhi = applicationGadzhi;
        }

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject => ApplicationGadzhi.
                                                                  FilesInfoProject.
                                                                  StatusProcessingProject;

        /// <summary>
        /// Вычислить процент отконвертированных файлов
        /// </summary>
        public int CalculatePercentageOfComplete()
        {
            double percentageOfComplete = 0;
            if (ApplicationGadzhi.FilesInfoProject?.StatusProcessingProject == StatusProcessingProject.Converting)
            {
                double numberOfComplete = ApplicationGadzhi.FilesInfoProject.
                                       FilesInfo.Count(file => file.StatusProcessing == StatusProcessing.Completed ||
                                                               file.StatusProcessing == StatusProcessing.Error);
                double numberTotal = ApplicationGadzhi.FilesInfoProject.
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
                                                 ConvertStatusProcessingProjectToString(ApplicationGadzhi.
                                                                                        FilesInfoProject.
                                                                                        StatusProcessingProject);

            if (ApplicationGadzhi.FilesInfoProject.StatusProcessingProject == StatusProcessingProject.Converting)
            {
                statusProcessingProjectName += $". Выполнено {CalculatePercentageOfComplete()}%";
            }

            return statusProcessingProjectName;
        }
    }
}
