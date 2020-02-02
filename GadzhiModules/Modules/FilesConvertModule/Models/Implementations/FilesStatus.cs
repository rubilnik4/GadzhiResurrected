using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании для всего пакета
    /// </summary>
    public class FilesStatus
    {
        public FilesStatus(IEnumerable<FileStatus> fileData,
                           StatusProcessingProject statusProcessingProject,
                           bool isConvertingChanged = false)
        {
            FileStatus = fileData;
            IsConvertingChanged = isConvertingChanged;
            StatusProcessingProject = statusProcessingProject;
        }

        /// <summary>
        /// Список файлов для изменения статуса
        /// </summary>
        public IEnumerable<FileStatus> FileStatus { get; }

        /// <summary>
        /// Изменился ли статус запуска/остановки процесса конвертирования
        /// </summary>
        public bool IsConvertingChanged { get; }

        /// <summary>
        /// Статус обработки пакета
        /// </summary>
        public StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Список файлов для изменения статуса
        /// </summary>
        public bool IsValid => FileStatus?.Any() == true;
    }
}
