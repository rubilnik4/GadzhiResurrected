namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        /// <summary>
        /// Время через которое осуществляются промежуточные запросы к серверу для проверки статуса файлов
        /// </summary>
        public int IntervalSecondsToIntermediateResponse => 2;

        /// <summary>
        /// Имя папка, куда копируются отконвертированные файлы
        /// </summary>
        public string DirectoryForSavingConvertedFiles => "Converted";
    }
}
