using System.Collections.Generic;
using System.Linq;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects
{
    /// <summary>
    /// Отправка изменений для реактивного свойства
    /// </summary>
    public class FilesChange
    {
        public FilesChange(IEnumerable<FileData> filesDataProject,
                           IEnumerable<FileData> fileData,
                           ActionType actionType,
                           bool isStatusProcessingProjectChanged)
        {
            FilesDataProject = filesDataProject ?? Enumerable.Empty<FileData>();
            FileData = fileData ?? Enumerable.Empty<FileData>();
            ActionType = actionType;
            IsStatusProcessingProjectChanged = isStatusProcessingProjectChanged;
        }

        /// <summary>
        /// Текущая модель
        /// </summary>
        public IEnumerable<FileData> FilesDataProject { get; }

        /// <summary>
        /// Список изменяемых файлов
        /// </summary>
        public IEnumerable<FileData> FileData { get; }

        /// <summary>
        /// Тип действия
        /// </summary>
        public ActionType ActionType { get; }

        /// <summary>
        /// Изменился ли статус обработки пакета
        /// </summary>
        public bool IsStatusProcessingProjectChanged { get; }
    }
}
