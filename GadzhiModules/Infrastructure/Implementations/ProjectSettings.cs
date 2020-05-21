using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ConvertingSettings;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        public ProjectSettings()
        {
            ConvertingSettings = new ConvertingSettings();
        }

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IConvertingSettings ConvertingSettings { get; private set; }

        /// <summary>
        /// Время через которое осуществляются промежуточные запросы к серверу для проверки статуса файлов
        /// </summary>
        public static int IntervalSecondsToIntermediateResponse => 2;

        /// <summary>
        /// Имя папка, куда копируются отконвертированные файлы
        /// </summary>
        public static string DirectoryForSavingConvertedFiles => "Converted";

        /// <summary>
        /// Обновить параметры конвертации
        /// </summary>
        public void UpdateConvertingSettings(string department) =>
            ConvertingSettings = new ConvertingSettings(department);
    }
}
