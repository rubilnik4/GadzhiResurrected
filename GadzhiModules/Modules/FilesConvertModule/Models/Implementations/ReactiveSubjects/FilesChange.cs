using System.Collections.Generic;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects
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
            FilesDataProject = filesDataProject;
            FileData = fileData;
            ActionType = actionType;
            IsStatusProcessingProjectChanged = isStatusProcessingProjectChanged;
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
        /// Изменился ли статус обработки пакета
        /// </summary>
        public bool IsStatusProcessingProjectChanged { get; }
    }
}
