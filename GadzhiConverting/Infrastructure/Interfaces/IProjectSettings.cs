using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
