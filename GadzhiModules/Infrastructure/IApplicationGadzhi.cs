using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure
{
    public interface IApplicationGadzhi
    {

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        Task AddFromFiles();

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>       
        Task AddFromFolders();

        /// <summary>
        /// Очистить список файлов
        /// </summary>       
        Task ClearFiles();

        /// <summary>
        /// Удалить файлы
        /// </summary>
        Task RemoveFiles();
    }
}
