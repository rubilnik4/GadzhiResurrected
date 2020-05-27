using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        public ProjectSettings(IConvertingSettings convertingSettings)
        {
            ConvertingSettings = convertingSettings;
        }

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IConvertingSettings ConvertingSettings { get; }

        /// <summary>
        /// Время через которое осуществляются промежуточные запросы к серверу для проверки статуса файлов
        /// </summary>
        public static int IntervalSecondsToIntermediateResponse => 2;

        /// <summary>
        /// Имя папка, куда копируются отконвертированные файлы
        /// </summary>
        public static string DirectoryForSavingConvertedFiles => "Converted";
    }
}
