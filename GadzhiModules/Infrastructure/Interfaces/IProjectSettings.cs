using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public interface IProjectSettings
    {
        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IConvertingSettings ConvertingSettings { get; }
    }
}
