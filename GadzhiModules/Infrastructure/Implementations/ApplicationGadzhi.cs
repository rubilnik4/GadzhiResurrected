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
            if (FilesInfoProject?.FilesInfo?.Any() == true)
            {
                var filesInSending = await GetFilesInSending();

                 FilesInfoProject.ChangeFilesStatusAndMarkError(filesInSending);

                var filesData = await GetFilesToRequest();
                if (filesData != null && filesData.Any())
                {
                    var filesDataRequest = new FilesDataRequest()
                    {
                        FilesData = filesData,
                    };

                    FilesDataIntermediateResponse response = await FileConvertingService.SendFiles(filesDataRequest);

                    var filesNotFound = GetFilesNotFound(filesData);
                    var filesChangedStatus = GetFilesStatusAfterUpload(response);
                    var filesUnion = filesNotFound?.Union(filesChangedStatus);

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
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        private Task<IEnumerable<FileStatus>> GetFilesInSending()
        {
            var filesInSending = FilesInfoProject?.
                                       FilesInfo?.
                                       Select(file => new FileStatus(file.FilePath, StatusProcessing.Sending));

            return Task.FromResult(filesInSending);
        }

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        private IEnumerable<FileStatus> GetFilesNotFound(IEnumerable<FileDataRequest> fileDataRequest)
        {
            var fileDataRequestPaths = fileDataRequest?.Select(fileRequest => fileRequest.FilePath);
            var filesNotFound = FilesInfoProject?.
                                 FilesInfoPath.
                                 Where(filePath => fileDataRequestPaths?.Contains(filePath) == false).
                                 Select(filePath => new FileStatus(filePath, StatusProcessing.Error, FileConvertErrorType.FileNotFound));

            return filesNotFound;
        }

        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        private IEnumerable<FileStatus> GetFilesStatusAfterUpload(FilesDataIntermediateResponse fileDataResponse)
        {
            var filesStatusAfterUpload = fileDataResponse?.
                                     FilesData.
                                     Select(fileResponse => FilesDataFromDTOConverter.ConvertToFileStatus(fileResponse));

            return filesStatusAfterUpload;
        }

        #endregion
    }
}
