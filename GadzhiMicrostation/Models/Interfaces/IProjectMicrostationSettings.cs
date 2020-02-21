using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Interfaces
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public interface IProjectMicrostationSettings
    {
        /// <summary>
        /// Список допустимых расширений для конвертации
        /// </summary>
        IList<string> AllowedFileTypes { get; }
    }
}
