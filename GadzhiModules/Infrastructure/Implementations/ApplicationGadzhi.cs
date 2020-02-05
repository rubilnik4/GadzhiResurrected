using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System.Windows;
using GadzhiDTO.Contracts.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using System.Reactive.Linq;
using System;
using GadzhiCommon.Helpers.Dialogs;
using System.Reactive.Disposables;
using ChannelAdam.ServiceModel;
using GadzhiModules.Infrastructure.Implementations.Information;

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
        /// Текущий статус конвертирования
        /// </summary>        
        private IStatusProcessingInformation StatusProcessingInformation { get; }

        /// <summary>
        /// Получение файлов для изменения статуса
        /// </summary>     
        private IFileDataProcessingStatusMark FileDataProcessingStatusMark { get; }

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private IProjectSettings ProjectSettings { get; }

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary>        
        public IExecuteAndCatchErrors ExecuteAndCatchErrors { get; set; }

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSystemOperations fileSystemOperations,
                                 IFilesData filesInfoProject,
                                 IServiceConsumer<IFileConvertingService> fileConvertingService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IStatusProcessingInformation statusProcessingInformation,
                                 IExecuteAndCatchErrors executeAndCatchErrors,
                                 IProjectSettings projectSettings)
        {
            DialogServiceStandard = dialogServiceStandard;
            FileSystemOperations = fileSystemOperations;
            FilesInfoProject = filesInfoProject;
            FileConvertingService = fileConvertingService;
            FileDataProcessingStatusMark = fileDataProcessingStatusMark;
            StatusProcessingInformation = statusProcessingInformation;
            ExecuteAndCatchErrors = executeAndCatchErrors;
            ProjectSettings = projectSettings;

            StatusProcessingUpdaterSubsriptions = new CompositeDisposable();
        }

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
            if (!StatusProcessingInformation.IsConverting)
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
            if (!StatusProcessingInformation.IsConverting)
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
            if (!StatusProcessingInformation.IsConverting)
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
            if (!StatusProcessingInformation.IsConverting)
            {
                FilesInfoProject.ClearFiles();
            }
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> filesToRemove)
        {
            if (!StatusProcessingInformation.IsConverting)
            {
                FilesInfoProject.RemoveFiles(filesToRemove);
            }
        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            if (StatusProcessingInformation.IsConverting)
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
            if (!StatusProcessingInformation.IsConverting)
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
            StatusProcessingInformation.ChangeFilesDataByStatus(filesStatusInSending);

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
            StatusProcessingInformation.ChangeFilesDataByStatus(filesStatusAfterSending);
        }

        /// <summary>
        /// Подписаться на изменение статуса файлов при конвертировании
        /// </summary>
        private void SubsribeToIntermediateResponse()
        {
            StatusProcessingUpdaterSubsriptions.Add(Observable.
                                                    Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToToIntermediateResponse)).
                                                    TakeWhile(_ => StatusProcessingInformation.IsConverting && !IsIntermediateResponseInProgress).
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
            FilesStatus filesStatus = await FileDataProcessingStatusMark.GetFilesStatusIntermediateResponse(filesDataIntermediateResponse);
            StatusProcessingInformation.ChangeFilesDataByStatus(filesStatus);

            if (filesDataIntermediateResponse.IsCompleted)
            {
                ClearSubsriptions();
                await GetCompleteFiles();
            }

            IsIntermediateResponseInProgress = false;
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        private async Task GetCompleteFiles()
        {

            FilesDataResponse filesDataResponse = await FileConvertingService.
                                                        Operations.
                                                        GetCompleteFiles(FilesInfoProject.ID);

            var filesStatusBeforeWrite = await FileDataProcessingStatusMark.
                                         GetFilesStatusCompleteResponseBeforeWriting(filesDataResponse);           
            StatusProcessingInformation.ChangeFilesDataByStatus(filesStatusBeforeWrite);

            var filesStatusWrite = await FileDataProcessingStatusMark.
                                         GetFilesStatusCompleteResponseAndWritten(filesDataResponse);

            StatusProcessingInformation.ChangeFilesDataByStatus(filesStatusWrite);

        }

        /// <summary>
        /// Обозначить начало конвертации
        /// </summary>
        private void GetReadyPropertiesToConvert()
        {
            IsIntermediateResponseInProgress = false;
        }

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        public void AbortPropertiesConverting()
        {
            IsIntermediateResponseInProgress = false;
            ClearSubsriptions();
            StatusProcessingInformation.ClearFilesDataToInitialValues();
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
