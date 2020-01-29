using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public interface IApplicationGadzhi
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>       
        IFilesData FilesInfoProject { get; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        Task AddFromFiles();

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>       
        Task AddFromFolders();

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        Task AddFromFilesOrDirectories(IEnumerable<string> filesToRemove);   

        /// <summary>
        /// Очистить список файлов
        /// </summary>       
        void ClearFiles();

        /// <summary>
        /// Удалить файлы
        /// </summary>
        void RemoveFiles(IEnumerable<FileData> fileOrDirectoriesPaths);

        /// <summary>
        /// Конвертировать файлы на сервре
        /// </summary>
        Task ConvertingFiles();

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();          
    }
}
