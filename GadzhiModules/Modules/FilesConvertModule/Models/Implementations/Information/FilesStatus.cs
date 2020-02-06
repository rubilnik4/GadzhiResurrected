using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании для всего пакета
    /// </summary>
    public class FilesStatus
    {
        public FilesStatus(IEnumerable<FileStatus> fileData,
                           StatusProcessingProject statusProcessingProject,
                           FilesQueueStatus filesQueueStatus = null)
        {
            FileStatus = fileData;         
            StatusProcessingProject = statusProcessingProject;
            FilesQueueStatus = filesQueueStatus;
        }

        /// <summary>
        /// Список файлов для изменения статуса
        /// </summary>
        public IEnumerable<FileStatus> FileStatus { get; }

        /// <summary>
        /// Статус обработки пакета
        /// </summary>
        public StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Информация о количестве файлов в очереди на сервере
        /// </summary>
        public FilesQueueStatus FilesQueueStatus { get; }

        /// <summary>
        /// Список файлов для изменения статуса
        /// </summary>
        public bool IsValid => FileStatus?.Any() == true;
    }
}
