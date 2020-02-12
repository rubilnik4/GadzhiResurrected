using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                return gadzhiPath + "\\"+
                       SettingsServer.DataBaseDirectoryDefault + "\\" +
                       SettingsServer.DataBaseNameDefault;
            }
        }
    }
}
