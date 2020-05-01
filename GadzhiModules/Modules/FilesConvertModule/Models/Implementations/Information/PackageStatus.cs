using GadzhiCommon.Enums.FilesConvert;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании для всего пакета
    /// </summary>
    public class PackageStatus
    {
        public PackageStatus(IEnumerable<FileStatus> fileData, StatusProcessingProject statusProcessingProject, 
                             QueueStatus queueStatus = new QueueStatus())
        {
            FileStatus = fileData ?? Enumerable.Empty<FileStatus>();
            StatusProcessingProject = statusProcessingProject;
            QueueStatus = queueStatus;
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
        public QueueStatus QueueStatus { get; }

        /// <summary>
        /// Список файлов для изменения статуса
        /// </summary>
        public bool IsValid => FileStatus?.Any() == true;
    }
}
