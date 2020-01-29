using GadzhiModules.Infrastructure.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using System.Windows;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiModules.Helpers.Converters.DTO;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;

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
        private IFileSeach FileSeach { get; }

        /// <summary>
        /// Получение файлов для изменения статуса
        /// </summary>     
        private IFileDataProcessingStatusMark FileDataProcessingStatusMark { get; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSeach fileSeach,
                                 IFilesData filesInfoProject,
                                 IFileConvertingService fileConvertingService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSeach = fileSeach;
            FilesInfoProject = filesInfoProject;
            FileConvertingService = fileConvertingService;
            FileDataProcessingStatusMark = fileDataProcessingStatusMark;
        }

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
            var filePaths = fileOrDirectoriesPaths?.Where(f => FileSeach.IsFile(f));
            var directoriesPath = fileOrDirectoriesPaths?.Where(d => FileSeach.IsDirectory(d) &&
                                                                     FileSeach.IsDirectoryExist(d));
            var filesInDirectories = directoriesPath?.Union(directoriesPath?.SelectMany(d => FileSeach.GetDirectories(d)))?
                                                     .SelectMany(d => FileSeach.GetFiles(d));
            var allFilePaths = filePaths?.Union(filesInDirectories)?
                                         .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f) &&
                                                     FileSeach.IsFileExist(f));
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
        /// Конвертировать ф-айлы на сервре
        /// </summary>
        public async Task ConvertingFiles()
        {
            if (FilesInfoProject?.FilesInfo?.Any() == true)
            {
                var filesInSending = await FileDataProcessingStatusMark.GetFilesInSending();

                FilesInfoProject.ChangeFilesStatusAndMarkError(filesInSending);

                var filesData = await FileDataProcessingStatusMark.GetFilesToRequest();
                if (filesData != null && filesData.Any())
                {
                    var filesDataRequest = new FilesDataRequest()
                    {
                        FilesData = filesData,
                    };

                    FilesDataIntermediateResponse response = await FileConvertingService.SendFiles(filesDataRequest);

                    var filesNotFound = FileDataProcessingStatusMark.GetFilesNotFound(filesData);
                    var filesChangedStatus = FileDataProcessingStatusMark.GetFilesStatusAfterUpload(response);
                    await Task.WhenAll(filesNotFound, filesChangedStatus);
                    var filesUnion = filesNotFound?.Result.Union(filesChangedStatus.Result);

                    FilesInfoProject.ChangeFilesStatusAndMarkError(filesUnion);
                }
            }
            else
            {
                DialogServiceStandard.ShowMessage("Необходимо загрузить файлы для конвертирования");
            }
        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
