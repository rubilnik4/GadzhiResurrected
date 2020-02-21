using GadzhiMicrostation.Models.Interfaces;
using System.Collections.Generic;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public class ProjectMicrostationSettings : IProjectMicrostationSettings
    {
        public ProjectMicrostationSettings()
        {
        }

        /// <summary>
        /// Список допустимых расширений для конвертации
        /// </summary>
        public IList<string> AllowedFileTypes => new List<string>()
        {
            "dgn",
        };
    }
}
