using System.Collections.Generic;
using System.Linq;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects
{
    /// <summary>
    /// Отправка изменений для реактивного свойства
    /// </summary>
    public class FilesChange
    {
        public FilesChange(IReadOnlyCollection<IFileData> filesDataProject, IFileData fileData,
                           ActionType actionType, bool isStatusProcessingProjectChanged)
            :this(filesDataProject, new List<IFileData>{ fileData }, actionType , isStatusProcessingProjectChanged)
        { }

        public FilesChange(IReadOnlyCollection<IFileData> filesDataProject, IEnumerable<IFileData> filesData,
                           ActionType actionType, bool isStatusProcessingProjectChanged)
        {
            FilesDataProject = filesDataProject ?? new List<IFileData>().AsReadOnly();
            FilesData = filesData?.ToList().AsReadOnly() ?? new List<IFileData>().AsReadOnly();
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
        public IReadOnlyCollection<IFileData> FilesData { get; }

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
