using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Implementations.Information
{
    /// <summary>
    /// Информация о количестве файлов в очереди на сервере
    /// </summary>
    public class FilesQueueInfo
    {
        public FilesQueueInfo()
        {
            Initialize();
        }

        /// <summary>
        /// Инициализировать начальные переменные
        /// </summary>
        private void Initialize()
        {
            FilesInQueueCount = 0;
            PackagesInQueueCount = 0;
            FirstFilesCountInQueue = null;
        }

        /// <summary>
        /// Количество файлов в очереди
        /// </summary>     
        public int FilesInQueueCount { get; private set; }

        /// <summary>
        /// Количество пакетов в очереди
        /// </summary>      
        public int PackagesInQueueCount { get; private set; }

        /// <summary>
        /// Первоначальное количество файлов в очереди. Для расчета процентов выполнения
        /// </summary>
        public int? FirstFilesCountInQueue { get; private set; }

        /// <summary>
        /// Заполнить поля исходя из промежуточного запроса
        /// </summary>        
        public void ChangeByFileQueueStatus(FilesQueueStatus filesQueueStatus, StatusProcessingProject statusProcessingProject)
        {
            switch (statusProcessingProject)
            {
                case StatusProcessingProject.Sending:
                    Initialize();
                    break;
                case StatusProcessingProject.InQueue:
                    if (filesQueueStatus != null)
                    {
                        FilesInQueueCount = filesQueueStatus.FilesInQueueCount;
                        PackagesInQueueCount = filesQueueStatus.PackagesInQueueCount;
                        if (FirstFilesCountInQueue == null)
                        {
                            FirstFilesCountInQueue = filesQueueStatus.FilesInQueueCount;
                        }
                    }
                    break;
                case StatusProcessingProject.End:
                    Initialize();
                    break;
            }

                 
        }
    }
}
