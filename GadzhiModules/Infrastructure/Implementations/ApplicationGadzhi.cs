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

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary>        
        public IExecuteAndCatchErrors ExecuteAndCatchErrors { get; set; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSystemOperations fileSystemOperations,
                                 IFilesData filesInfoProject,
                                 IServiceConsumer<IFileConvertingService> fileConvertingService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IExecuteAndCatchErrors executeAndCatchErrors)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSystemOperations = fileSystemOperations;
            FilesInfoProject = filesInfoProject;
            FileConvertingService = fileConvertingService;
            FileDataProcessingStatusMark = fileDataProcessingStatusMark;
            ExecuteAndCatchErrors = executeAndCatchErrors;

            StatusProcessingUpdaterSubsriptions = new CompositeDisposable();
        }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        public bool IsConverting { get; private set; }

        /// <summary>
        /// Выполняется ли промежуточный запрос
        /// </summary 
        private bool IsIntermediateResponseInProgress { get; set; }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов. Таймер с подпиской
        /// </summary>
        private CompositeDisposable StatusProcessingUpdaterSubsriptions { get; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            if (!IsConverting)
            {
                var filePaths = await DialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);
                await AddFromFilesOrDirectories(filePaths);
            }
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            if (!IsConverting)
            {
                var directoryPaths = await DialogServiceStandard.OpenFolderDialog(true);
                await AddFromFilesOrDirectories(directoryPaths);
            }
        }

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        public async Task AddFromFilesOrDirectories(IEnumerable<string> fileOrDirectoriesPaths)
        {
            if (!IsConverting)
            {
                var allFilePaths = await FileSystemOperations.GetFilesFromDirectoryAndSubDirectory(fileOrDirectoriesPaths);
                if (allFilePaths != null && allFilePaths.Any())
                {
                    FilesInfoProject.AddFiles(allFilePaths);
                }
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            if (!IsConverting)
            {
                FilesInfoProject.ClearFiles();
            }
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> filesToRemove)
        {
            if (!IsConverting)
            {
                FilesInfoProject.RemoveFiles(filesToRemove);
            }
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

        #region ConvertingFilesOnServer

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>
        public async Task ConvertingFiles()
        {
            if (!IsConverting)
            {
                GetReadyPropertiesToConvert();

                if (FilesInfoProject?.FilesInfo?.Any() == true)
                {
                    FilesDataRequest filesDataRequest = await PrepareFilesToSending();
                    if (filesDataRequest.IsValidToSend)
                    {
                        await SendFilesToConverting(filesDataRequest);

                        SubsribeToIntermediateResponse();
                    }
                }
                else
                {
                    AbortPropertiesConverting();
                    DialogServiceStandard.ShowMessage("Необходимо загрузить файлы для конвертирования");
                }
            }
        }

        /// <summary>
        /// Подготовить данные к отправке
        /// </summary>     
        private async Task<FilesDataRequest> PrepareFilesToSending()
        {
            var filesStatusInSending = await FileDataProcessingStatusMark.GetFilesInSending();
            FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusInSending);

            var filesDataRequest = await FileDataProcessingStatusMark.GetFilesDataToRequest();
            return filesDataRequest;
        }

        /// <summary>
        /// Отправить файлы для конвертации
        /// </summary>
        /// <returns></returns>
        private async Task SendFilesToConverting(FilesDataRequest filesDataRequest)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse = await FileConvertingService.
                                                                                           Operations.
                                                                                           SendFiles(filesDataRequest);
            var filesStatusAfterSending = await FileDataProcessingStatusMark.GetFilesStatusUnionAfterSendAndNotFound(filesDataRequest, filesDataIntermediateResponse);
            FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusAfterSending);
        }

        /// <summary>
        /// Подписаться на изменение статуса файлов при конвертировании
        /// </summary>
        private void SubsribeToIntermediateResponse()
        {
            StatusProcessingUpdaterSubsriptions.Add(Observable.
                                                    Interval(TimeSpan.FromSeconds(2)).
                                                    TakeWhile(_ => IsConverting && !IsIntermediateResponseInProgress).
                                                    Subscribe(async _ => await ExecuteAndCatchErrors.
                                                                               ExecuteAndHandleErrorAsync(UpdateStatusProcessing, AbortPropertiesConverting)));
        }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task UpdateStatusProcessing()
        {
            IsIntermediateResponseInProgress = true;

            FilesDataIntermediateResponse filesDataIntermediateResponse = await FileConvertingService.
                                                                                Operations.
                                                                                CheckFilesStatusProcessing(FilesInfoProject.ID);
            var filesStatusUnion = await FileDataProcessingStatusMark.GetFilesStatusIntermediateResponse(filesDataIntermediateResponse);
            FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusUnion);

            if (filesDataIntermediateResponse.IsCompleted)
            {
                ClearSubsriptions();
                await GetCompleteFiles(filesDataIntermediateResponse.IsCompleted);
            }

            IsIntermediateResponseInProgress = false;
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        private async Task GetCompleteFiles(bool isCompleted)
        {
            if (isCompleted)
            {
                FilesDataResponse filesDataResponse = await FileConvertingService.
                                                            Operations.
                                                            GetCompleteFiles(FilesInfoProject.ID);

                var filesStatusWright = await FileDataProcessingStatusMark.
                                              GetFilesStatusCompleteResponse(filesDataResponse);
                FilesInfoProject.ChangeFilesStatusAndMarkError(filesStatusWright);

                IsConverting = false;
            }
        }

        /// <summary>
        /// Обозначить начало конвертации
        /// </summary>
        private void GetReadyPropertiesToConvert()
        {
            IsConverting = true;
            IsIntermediateResponseInProgress = false;
        }

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        public void AbortPropertiesConverting()
        {
            IsConverting = false;
            IsIntermediateResponseInProgress = false;
            ClearSubsriptions();
            FilesInfoProject.ChangeAllFilesStatusAndMarkError();
        }

        /// <summary>
        /// Очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubsriptions()
        {
            StatusProcessingUpdaterSubsriptions?.Dispose();
        }
        #endregion

        public void Dispose()
        {
            ClearSubsriptions();
        }
    }
}
