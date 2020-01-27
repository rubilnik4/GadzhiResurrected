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

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public class ApplicationGadzhi : IApplicationGadzhi
    {
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        public IDialogServiceStandard DialogServiceStandard { get; }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        public IFileSeach FileSeach { get; }

        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        public IFilesData FilesInfoProject { get; }

        /// <summary>
        /// Сервис конвертации
        /// </summary>     
        public IFileConvertingService FileConvertingService { get; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSeach fileSeach,
                                 IFilesData filesInfoProject,
                                 IFileConvertingService fileConvertingService)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSeach = fileSeach;
            FilesInfoProject = filesInfoProject;
            FileConvertingService = fileConvertingService;
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
            var filesData = await GetFilesToRequest();

            if (filesData != null && filesData.Any())
            {
                var filesDataRequest = new FilesDataRequest()
                {
                    FilesData = filesData,
                };

                var response = await FileConvertingService.SendFiles(filesDataRequest);

                MarkUnavalableFilesWithError(response, filesDataRequest.FilesData);
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

        #region ConvertingFiles

        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        private async Task<IEnumerable<FileDataRequest>> GetFilesToRequest()
        {
            var filesRequestExist = await Task.WhenAll(FilesInfoProject?.FilesInfo?.Where(file => FileSeach.IsFileExist(file.FilePath))?.
                                                                                    Select(file => FilesDataToDTOConverter.ConvertToFileDataDTO(file, FileSeach)));
            var filesRequestEnsuredWithBytes = filesRequestExist?.Where(file => file.FileDataSource != null);

            return filesRequestEnsuredWithBytes;
        }

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        private void MarkUnavalableFilesWithError(bool serverResponse, IEnumerable<FileDataRequest> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);

            var filesWithError = FilesInfoProject?.FilesInfo.Where(file => serverResponse == false ||
                                                      fileDataRequestPaths?.Contains(file.FilePath) == false);

        }

        #endregion
    }
}
