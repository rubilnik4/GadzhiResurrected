using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model.ReactiveSubjects
{
    /// <summary>
    /// Отправка изменений для реактивного свойства
    /// </summary>
    public class FileChange
    {
        public FileChange(IEnumerable<FileData> fileData, ActionType actionType)
        {
            FileData = fileData;
            ActionType = actionType;
        }

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
