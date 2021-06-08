using System.Collections.Generic;
using System.Reactive.Subjects;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiResurrected.Infrastructure.Interfaces.ApplicationGadzhi
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
        void AddFromFiles();

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>       
        void AddFromFolders();

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        void AddFromFilesOrDirectories(IEnumerable<string> filesToRemove);

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