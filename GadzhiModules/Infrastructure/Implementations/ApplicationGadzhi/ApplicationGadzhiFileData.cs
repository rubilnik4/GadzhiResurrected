using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using GadzhiCommon.Helpers.Dialogs;
using GadzhiModules.Infrastructure.Implementations.Validates;
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
        public ISubject<FilesChange> FileDataChange => _packageData.FileDataChange;

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public void AddFromFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var filePaths = _dialogService.OpenFileDialog(true, DialogFilters.DocAndDgn);
            AddFromFilesOrDirectories(filePaths);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public void AddFromFolders()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var directoryPaths = _dialogService.OpenFolderDialog(true);
            AddFromFilesOrDirectories(directoryPaths);
        }

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        public void AddFromFilesOrDirectories(IEnumerable<string> fileOrDirectoriesPaths)
        {
            if (_statusProcessingInformation.IsConverting) return;

            var allFilePaths = _filePathOperations.GetFilesFromPaths(fileOrDirectoriesPaths).ToList();
            _packageData.AddFiles(allFilePaths, _projectSettings.ConvertingSettings.ColorPrintType);
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;

            _packageData.ClearFiles();
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<IFileData> filesToRemove)
        {
            if (_statusProcessingInformation.IsConverting) return;

            _packageData.RemoveFiles(filesToRemove);
        }
    }
}