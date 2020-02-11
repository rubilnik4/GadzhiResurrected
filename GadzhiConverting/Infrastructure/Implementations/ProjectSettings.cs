using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приолжения
    /// </summary>
    public class ProjectSettings: IProjectSettings
    {
        /// <summary>
        /// Путь к файлу базы данных SQLite
        /// </summary>
        public string SQLiteDataBasePath => ConfigurationManager.
                                            ConnectionStrings["SQLiteConnvectionString"].
                                            ConnectionString;
    }
}
