using System.Collections.Generic;
using System.Linq;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects
{
    /// <summary>
    /// Отправка изменений для реактивного свойства
    /// </summary>
    public class FilesChange
    {
        public FilesChange(IReadOnlyCollection<IFileData> filesDataProject,
                           IEnumerable<IFileData> fileData,
                           ActionType actionType,
                           bool isStatusProcessingProjectChanged)
        {
            FilesDataProject = filesDataProject ?? new List<IFileData>().AsReadOnly();
            FileData = fileData?.ToList().AsReadOnly() ?? new List<IFileData>().AsReadOnly();
            ActionType = actionType;
            IsStatusProcessingProjectChanged = isStatusProcessingProjectChanged;
        }

        /// <summary>
        /// Текущая модель
        /// </summary>
        public IReadOnlyCollection<IFileData> FilesDataProject { get; }

        /// <summary>
        /// Список изменяемых файлов
        /// </summary>
        public IReadOnlyCollection<IFileData> FileData { get; }

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
