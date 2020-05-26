using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi
{
    /// <summary>
    /// Слой приложения, инфраструктура. Файлы данных
    /// </summary>
    public interface IApplicationGadzhiFileData
    {
        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        ISubject<FilesChange> FileDataChange { get; }

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
        void RemoveFiles(IEnumerable<IFileData> fileOrDirectoriesPaths);
    }
}