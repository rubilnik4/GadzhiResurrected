using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects
{
    /// <summary>
    /// Отправка изменений для реактивного свойства
    /// </summary>
    public class FilesChange
    {
        public FilesChange(IEnumerable<FileData> filesDataProject, IEnumerable<FileData> fileData, ActionType actionType)
        {
            FilesDataProject = filesDataProject;
            FileData = fileData;
            ActionType = actionType;
        }

        /// <summary>
        /// Текущая модель
        /// </summary>
        public IEnumerable<FileData> FilesDataProject { get; }

        /// <summary>
        /// Список изменямых файлов
        /// </summary>
        public IEnumerable<FileData> FileData { get; }

        /// <summary>
        /// Тип действия
        /// </summary>
        public ActionType ActionType { get; }

        /// <summary>
        /// Изменился ли статус запуска/остановки процесса конвертирования
        /// </summary>
        public bool IsConvertingChanged { get; set; }

        /// <summary>
        /// Изменился ли статус выполнения пакета конвертирования
        /// </summary>
        public bool IsStatusProjectChanged { get; set; }
    }
}
