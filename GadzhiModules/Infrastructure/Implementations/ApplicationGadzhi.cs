using GadzhiModules.Infrastructure.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System.Windows;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiModules.Helpers.Converters.DTO;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using System.Reactive.Linq;
using System;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public class ApplicationGadzhi : IApplicationGadzhi
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        public IFilesData FilesInfoProject { get; }

        /// <summary>
        /// Сервис конвертации
        /// </summary>     
        private IFileConvertingService FileConvertingService { get; }

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private IDialogServiceStandard DialogServiceStandard { get; }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private IFileSystemOperations FileSystemOperations { get; }

        /// <summary>
        /// Получение файлов для изменения статуса
        /// </summary>     
        private IFileDataProcessingStatusMark FileDataProcessingStatusMark { get; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSystemOperations fileSystemOperations,
                                 IFilesData filesInfoProject,
                                 IFileConvertingService fileConvertingService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSystemOperations = fileSystemOperations;
            FilesInfoProject = filesInfoProject;
            FileConvertingService = fileConvertingService;
            FileDataProcessingStatusMark = fileDataProcessingStatusMark;
        }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        public bool IsConverting { get; private set; }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов. Таймер с подпиской
        /// </summary>
        private IDisposable StatusProcessingUpdater => Observable.                                
                                                       Interval(TimeSpan.FromSeconds(10)).
                                                       TakeWhile(_ => IsConverting).
                                                       Subscribe(_ => UpdateStatusProseccing());

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            var filePaths = await DialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);
            await AddFromFilesOrDirectories(filePaths);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            var directoryPaths = await DialogServiceStandard.OpenFolderDialog(true);
            await AddFromFilesOrDirectories(directoryPaths);
        }

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        public async Task AddFromFilesOrDirectories(IEnumerable<string> fileOrDirectoriesPaths)
        {
            //Поиск файлов на один уровень ниже и в текущей папке          
            var filePaths = fileOrDirectoriesPaths?.Where(f => FileSystemOperations.IsFile(f));
            var directoriesPath = fileOrDirectoriesPaths?.Where(d => FileSystemOperations.IsDirectory(d) &&
                                                                     FileSystemOperations.IsDirectoryExist(d));
            var filesInDirectories = directoriesPath?.Union(directoriesPath?.SelectMany(d => FileSystemOperations.GetDirectories(d)))?
                                                     .SelectMany(d => FileSystemOperations.GetFiles(d));
            var allFilePaths = filePaths?.Union(filesInDirectories)?
                                         .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f) &&
                                                     FileSystemOperations.IsFileExist(f));
            await Task.FromResult(allFilePaths);

            if (allFilePaths != null && allFilePaths.Any())
            {
                FilesInfoProject.AddFiles(allFilePaths);
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            FilesInfoProject.ClearFiles();
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> filesToRemove)
        {
            FilesInfoProject.RemoveFiles(filesToRemove);
        }

        /// <summary>
        /// Конвертировать файлы на сервере
        /// </summary>
        public async Task ConvertingFiles()
        {
            IsConverting = true;

            if (FilesInfoProject?.FilesInfo?.Any() == true)
            {
                var filesInSending = await FileDataProcessingStatusMark.GetFilesInSending();

                FilesInfoProject.ChangeFilesStatusAndMarkError(filesInSending);

                var filesDataRequest = await FileDataProcessingStatusMark.GetFilesDataToRequest();
                if (filesDataRequest.IsValidToSend)
                { 
                    FilesDataIntermediateResponse filesDataIntermediateResponse = await FileConvertingService.SendFiles(filesDataRequest);

                    var filesStatusUnion = await FileDataProcessingStatusMark.GetFileStatusUnionAfterSendAndNotFound(filesDataRequest, filesDataIntermediateResponse);
                    FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusUnion);
                }
            }
            else
            {
                DialogServiceStandard.ShowMessage("Необходимо загрузить файлы для конвертирования");
            }
        }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private void UpdateStatusProseccing ()
        {

        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            if (IsConverting)
            {
                if (!DialogServiceStandard.ShowMessageOkCancel("Бросить все на полпути?"))
                {
                    return;
                }
            }

            Application.Current.Shutdown();
        }
    }
}
