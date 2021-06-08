using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании для всего пакета
    /// </summary>
    public class PackageStatus
    {
        public PackageStatus(IEnumerable<FileStatus> fileData, StatusProcessingProject statusProcessingProject, 
                             QueueStatus queueStatus = new QueueStatus())
        {
            FileStatus = fileData?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(fileData));
            StatusProcessingProject = statusProcessingProject;
            QueueStatus = queueStatus;
        }

        /// <summary>
        /// Список файлов для изменения статуса
        /// </summary>
        public IReadOnlyCollection<FileStatus> FileStatus { get; }

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
