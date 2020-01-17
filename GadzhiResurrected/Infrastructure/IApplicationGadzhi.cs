using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhieResurrected.Infrastructure
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
        Task AddFromFolder();

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
