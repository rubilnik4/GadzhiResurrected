using GadzhiModules.Modules.FilesConvertModule.Model;
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
        /// Модель конвертируемых файлов
        /// </summary>       
        FilesData FilesInfoProject { get; }

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
        void ClearFiles();

        /// <summary>
        /// Удалить файлы
        /// </summary>
        void RemoveFiles();
    }
}
