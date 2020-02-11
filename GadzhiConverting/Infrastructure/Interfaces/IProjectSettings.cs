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
    }
}
