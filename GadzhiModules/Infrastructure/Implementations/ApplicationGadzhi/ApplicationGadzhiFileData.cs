using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using GadzhiCommon.Helpers.Dialogs;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Infrastructure.Implementations.ApplicationGadzhi
{
    /// <summary>
    /// Слой приложения, инфраструктура. Файлы данных
    /// </summary>
    public partial class ApplicationGadzhi
    {
        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange => _packageInfoProject.FileDataChange;

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var filePaths = await _dialogService.OpenFileDialog(true, DialogFilters.DocAndDgn);
            await AddFromFilesOrDirectories(filePaths);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var directoryPaths = await _dialogService.OpenFolderDialog(true);
            await AddFromFilesOrDirectories(directoryPaths);
        }

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        public async Task AddFromFilesOrDirectories(IEnumerable<string> fileOrDirectoriesPaths)
        {
            if (_statusProcessingInformation.IsConverting) return;

            var allFilePaths = await Task.FromResult(_fileSystemOperations.GetFilesFromPaths(fileOrDirectoriesPaths));
            _packageInfoProject.AddFiles(allFilePaths);
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;

            _packageInfoProject.ClearFiles();
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<IFileData> filesToRemove)
        {
            if (_statusProcessingInformation.IsConverting) return;

            _packageInfoProject.RemoveFiles(filesToRemove);
        }
    }
}