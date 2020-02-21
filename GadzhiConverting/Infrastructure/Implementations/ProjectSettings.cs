using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приолжения
    /// </summary>
    public class ProjectSettings : IProjectSettings
    {
        /// <summary>
        /// Использовать стандартный путь к базе данных
        /// </summary>
        public bool UseDefaultSQLiteDataBase => true;

        /// <summary>
        /// Путь к файлу базы данных SQLite
        /// </summary>
        public string SQLiteDataBasePath
        {
            get
            {
                string dataBasePath = UseDefaultSQLiteDataBase ?
                                     SQLiteDataBasePathByDefault :
                                     SQLiteDataBasePathByConnection;
                return dataBasePath;
            }
        }

        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        public string ConvertingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                             "//Converting.gitignore";


        /// <summary>
        /// Путь к файлу базы данных SQLite через строку подключения
        /// </summary>
        private string SQLiteDataBasePathByConnection => ConfigurationManager.
                                                        ConnectionStrings["SQLiteConnvectionString"].
                                                        ConnectionString;

        /// <summary>
        /// Путь к файлу базы данных SQLite стандартный
        /// </summary>
        private string SQLiteDataBasePathByDefault
        {
            get
            {
                string applicationPath = Environment.CurrentDirectory;
                var directoryPath = new DirectoryInfo(applicationPath);
                var gadzhiPath = directoryPath.Parent.Parent.Parent.FullName;
                return gadzhiPath + "\\" +
                       SettingsServer.DataBaseDirectoryDefault + "\\" +
                       SettingsServer.DataBaseNameDefault;
            }
        }

        /// <summary>
        /// Время через которое осуществляется проверка пакетов на сервере
        /// </summary>
        public int IntervalSecondsToServer => 5;

        /// <summary>
        /// Получить хэш код приложения
        /// </summary>
        public int ApplicationHashCode
        {
            get
            {
                int hashCode = 17;
                hashCode = hashCode * 31 + Environment.CurrentDirectory.GetHashCode();
                hashCode = hashCode * 31 + Environment.UserDomainName.GetHashCode();
                hashCode = hashCode * 31 + Environment.UserName.GetHashCode();

                return hashCode;
            }
        }

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        public string NetworkName => Environment.UserDomainName + "\\" + Environment.MachineName;

    }
}
