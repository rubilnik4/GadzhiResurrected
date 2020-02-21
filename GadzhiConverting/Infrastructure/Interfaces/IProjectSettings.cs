namespace GadzhiConverting.Infrastructure.Interfaces
{
    public interface IProjectSettings
    {
        /// <summary>
        /// Путь к файлу базы данных SQLite
        /// </summary>
        string SQLiteDataBasePath { get; }

        /// <summary>
        /// Использовать стандартный путь к базе данных
        /// </summary>
        bool UseDefaultSQLiteDataBase { get; }

        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        string ConvertingDirectory { get; }

        /// <summary>
        /// Время через которое осуществляется проверка пакетов на сервере
        /// </summary>
        int IntervalSecondsToServer { get; }

        /// <summary>
        /// Получить хэш код приложения
        /// </summary>
        int ApplicationHashCode { get; }

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        string NetworkName { get; }
    }
}
