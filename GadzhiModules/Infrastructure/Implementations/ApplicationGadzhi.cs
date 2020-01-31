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
using GadzhiCommon.Helpers.Dialogs;
using System.Reactive.Disposables;
using System.ServiceModel;
using GadzhiDTO.Healpers;
using ChannelAdam.ServiceModel;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public class ApplicationGadzhi : IApplicationGadzhi, IDisposable
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        public IFilesData FilesInfoProject { get; }

        /// <summary>
        /// Сервис конвертации
        /// </summary>     
        private IServiceConsumer<IFileConvertingService> FileConvertingService { get; }

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
                                 IServiceConsumer<IFileConvertingService> fileConvertingService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSystemOperations = fileSystemOperations;
            FilesInfoProject = filesInfoProject;
            FileConvertingService = fileConvertingService;
            FileDataProcessingStatusMark = fileDataProcessingStatusMark;

            StatusProcessingUpdaterSubsriptions = new CompositeDisposable();
        }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        public bool IsConverting { get; private set; }

        /// <summary>
        /// Индикатор готовности файлов
        /// </summary 
        private bool IsComplited { get; set; }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов. Таймер с подпиской
        /// </summary>
        private CompositeDisposable StatusProcessingUpdaterSubsriptions { get; }

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
            var allFilePaths = await FileSystemOperations.GetFilesFromDirectoryAndSubDirectory(fileOrDirectoriesPaths);
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

        #region ConvertingFilesOnServer
        /// <summary>
        /// Конвертировать файлы на сервере
        /// </summary>
        public async Task ConvertingFiles()
        {
            GetReadyPropertiesToConvert();

            if (FilesInfoProject?.FilesInfo?.Any() == true)
            {
                var filesInSending = await FileDataProcessingStatusMark.GetFilesInSending();

                FilesInfoProject.ChangeFilesStatusAndMarkError(filesInSending);

                var filesDataRequest = await FileDataProcessingStatusMark.GetFilesDataToRequest();
                if (filesDataRequest.IsValidToSend)
                {
                    FilesDataIntermediateResponse filesDataIntermediateResponse = await FileConvertingService.
                                                                                        Operations.
                                                                                        SendFiles(filesDataRequest);
                    var filesStatusUnion = await FileDataProcessingStatusMark.GetFilesStatusUnionAfterSendAndNotFound(filesDataRequest, filesDataIntermediateResponse);

                    FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusUnion);

                    StatusProcessingUpdaterSubsriptions.Add(Observable.
                                                            Interval(TimeSpan.FromSeconds(2)).
                                                            TakeWhile(_ => IsConverting && !IsComplited).
                                                            Subscribe(async _ => await UpdateStatusProcessing()));
                }
            }
            else
            {
                AbortPropertiesConverting();

                DialogServiceStandard.ShowMessage("Необходимо загрузить файлы для конвертирования");
            }
        }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task UpdateStatusProcessing()
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse = await FileConvertingService.
                                                                                Operations.
                                                                                CheckFilesStatusProcessing(FilesInfoProject.ID);
            var filesStatusUnion = await FileDataProcessingStatusMark.GetFilesStatusIntermediateResponse(filesDataIntermediateResponse);
            FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusUnion);

            if (filesDataIntermediateResponse.IsComplited)
            {
                IsComplited = true;
                ClearSubsriptions();
                await GetCompliteFiles(filesDataIntermediateResponse.IsComplited);
            }
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        private async Task GetCompliteFiles(bool isComplited)
        {
            if (isComplited)
            {
                FilesDataResponse filesDataResponse = await FileConvertingService.
                                                            Operations.
                                                            GetCompliteFiles(FilesInfoProject.ID);

                var filesStatusWright = await FileDataProcessingStatusMark.GetFilesStatusCompliteResponse(filesDataResponse);                
                FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusWright);

                IsConverting = false;
            }
        }
        #endregion  

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

        /// <summary>
        /// Обозначить начало конвертации
        /// </summary>
        private void GetReadyPropertiesToConvert()
        {
            IsConverting = true;
            IsComplited = false;
        }

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        private void AbortPropertiesConverting()
        {
            IsConverting = false;
            IsComplited = false;
        }

        /// <summary>
        /// очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubsriptions()
        {
            StatusProcessingUpdaterSubsriptions?.Dispose();
        }
        public void Dispose()
        {
            ClearSubsriptions();
        }
    }
}
