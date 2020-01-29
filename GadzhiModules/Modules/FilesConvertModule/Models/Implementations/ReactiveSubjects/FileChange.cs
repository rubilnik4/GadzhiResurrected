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
    public class FileChange
    {
        public FileChange(IEnumerable<FileData> filesDataProject, IEnumerable<FileData> fileData, ActionType actionType)
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
    }
}
