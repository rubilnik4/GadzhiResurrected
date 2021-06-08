namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public interface IProjectSettings
    {
        /// <summary>
        /// Параметры конвертации
        /// </summary>
        IConvertingSettings ConvertingSettings { get; }
    }
}
