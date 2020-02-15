using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System.Windows;
using GadzhiCommon.Infrastructure.Interfaces;
using System.Reactive.Linq;
using System;
using GadzhiCommon.Helpers.Dialogs;
using System.Reactive.Disposables;
using ChannelAdam.ServiceModel;
using GadzhiModules.Infrastructure.Implementations.Information;
using System.Reactive.Subjects;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;

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
        private readonly IFilesData _filesInfoProject;

        /// <summary>
        /// Сервис конвертации
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingServiceClient> _fileConvertingServiceClient;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private readonly IDialogServiceStandard _dialogServiceStandard;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Текущий статус конвертирования
        /// </summary>        
        private readonly IStatusProcessingInformation _statusProcessingInformation;
        /// <summary>
        /// Получение файлов для изменения статуса
        /// </summary>     
        private readonly IFileDataProcessingStatusMark _fileDataProcessingStatusMark;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary>        
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов. Таймер с подпиской
        /// </summary>
        private readonly CompositeDisposable _statusProcessingUpdaterSubsriptions;

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSystemOperations fileSystemOperations,
                                 IFilesData filesInfoProject,
                                 IServiceConsumer<IFileConvertingServiceClient> fileConvertingServiceClient,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IStatusProcessingInformation statusProcessingInformation,
                                 IExecuteAndCatchErrors executeAndCatchErrors,
                                 IProjectSettings projectSettings)
        {
            _dialogServiceStandard = dialogServiceStandard;
            _fileSystemOperations = fileSystemOperations;
            _filesInfoProject = filesInfoProject;
            _fileConvertingServiceClient = fileConvertingServiceClient;
            _fileDataProcessingStatusMark = fileDataProcessingStatusMark;
            _statusProcessingInformation = statusProcessingInformation;
            _executeAndCatchErrors = executeAndCatchErrors;
            _projectSettings = projectSettings;

            _statusProcessingUpdaterSubsriptions = new CompositeDisposable();

           // SubsribeToIntermediateResponse();
        }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange => _filesInfoProject.FileDataChange;

        /// <summary>
        /// Выполняется ли промежуточный запрос
        /// </summary 
        private bool IsIntermediateResponseInProgress { get; set; }       

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            if (!_statusProcessingInformation.IsConverting)
            {
                var filePaths = await _dialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);
                await AddFromFilesOrDirectories(filePaths);
            }
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            if (!_statusProcessingInformation.IsConverting)
            {
                var directoryPaths = await _dialogServiceStandard.OpenFolderDialog(true);
                await AddFromFilesOrDirectories(directoryPaths);
            }
        }

        /// <summary>
        /// Добавить файлы или папки для конвертации
        /// </summary>
        public async Task AddFromFilesOrDirectories(IEnumerable<string> fileOrDirectoriesPaths)
        {
            if (!_statusProcessingInformation.IsConverting)
            {
                var allFilePaths = await _fileSystemOperations.GetFilesFromDirectoryAndSubDirectory(fileOrDirectoriesPaths);
                if (allFilePaths != null && allFilePaths.Any())
                {
                    _filesInfoProject.AddFiles(allFilePaths);
                }
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>  
        public void ClearFiles()
        {
            if (!_statusProcessingInformation.IsConverting)
            {
                _filesInfoProject.ClearFiles();
            }
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> filesToRemove)
        {
            if (!_statusProcessingInformation.IsConverting)
            {
                _filesInfoProject.RemoveFiles(filesToRemove);
            }
        }

        /// <summary>
        /// Обновить статус конвертирования
        /// </summary>
        public void ChangeFilesStatusAndMarkError(FilesStatus filesStatus)
        {

            _filesInfoProject.ChangeFilesStatus(filesStatus);
        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            if (_statusProcessingInformation.IsConverting)
            {
                if (!_dialogServiceStandard.ShowMessageOkCancel("Бросить все на полпути?"))
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
            if (!_statusProcessingInformation.IsConverting)
            {
                if (_filesInfoProject?.FilesInfo?.Any() == true)
                {
                    FilesDataRequestClient filesDataRequest = await PrepareFilesToSending();
                    if (filesDataRequest.IsValid)
                    {
                        await SendFilesToConverting(filesDataRequest);

                        SubsribeToIntermediateResponse();
                    }
                }
                else
                {
                    AbortPropertiesConverting();
                    _dialogServiceStandard.ShowMessage("Необходимо загрузить файлы для конвертирования");
                }
            }
        }

        /// <summary>
        /// Подготовить данные к отправке
        /// </summary>     
        private async Task<FilesDataRequestClient> PrepareFilesToSending()
        {
            var filesStatusInSending = await _fileDataProcessingStatusMark.GetFilesInSending();
            _filesInfoProject.ChangeFilesStatus(filesStatusInSending);

            var filesDataRequest = await _fileDataProcessingStatusMark.GetFilesDataToRequest();
            return filesDataRequest;
        }

        /// <summary>
        /// Отправить файлы для конвертации
        /// </summary>
        /// <returns></returns>
        private async Task SendFilesToConverting(FilesDataRequestClient filesDataRequest)
        {
            FilesDataIntermediateResponseClient filesDataIntermediateResponse = await _fileConvertingServiceClient.
                                                                                 Operations.
                                                                                 SendFiles(filesDataRequest);
            var filesStatusAfterSending = await _fileDataProcessingStatusMark.GetFilesStatusUnionAfterSendAndNotFound(filesDataRequest, filesDataIntermediateResponse);
            _filesInfoProject.ChangeFilesStatus(filesStatusAfterSending);
        }

        /// <summary>
        /// Подписаться на изменение статуса файлов при конвертировании
        /// </summary>
        private void SubsribeToIntermediateResponse()
        {
            _statusProcessingUpdaterSubsriptions.Add(Observable.
                                                    Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToIntermediateResponse)).
                                                    Where(_ => _statusProcessingInformation.IsConverting && !IsIntermediateResponseInProgress).
                                                    Subscribe(async _ => await _executeAndCatchErrors.
                                                                               ExecuteAndHandleErrorAsync(UpdateStatusProcessing, 
                                                                                                          () => IsIntermediateResponseInProgress = true,
                                                                                                          AbortPropertiesConverting,
                                                                                                          () => IsIntermediateResponseInProgress = false)));
        }

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task UpdateStatusProcessing()
        {
            FilesDataIntermediateResponseClient filesDataIntermediateResponse = await _fileConvertingServiceClient.
                                                                                Operations.
                                                                                CheckFilesStatusProcessing(_filesInfoProject.Id);
            FilesStatus filesStatus = await _fileDataProcessingStatusMark.GetFilesStatusIntermediateResponse(filesDataIntermediateResponse);
            _filesInfoProject.ChangeFilesStatus(filesStatus);

            if (filesDataIntermediateResponse.IsCompleted)
            {
                ClearSubsriptions();
                await GetCompleteFiles();
            }         
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        private async Task GetCompleteFiles()
        {

            FilesDataResponseClient filesDataResponse = await _fileConvertingServiceClient.
                                                        Operations.
                                                        GetCompleteFiles(_filesInfoProject.Id);

            var filesStatusBeforeWrite = await _fileDataProcessingStatusMark.
                                         GetFilesStatusCompleteResponseBeforeWriting(filesDataResponse);
            _filesInfoProject.ChangeFilesStatus(filesStatusBeforeWrite);

            var filesStatusWrite = await _fileDataProcessingStatusMark.
                                         GetFilesStatusCompleteResponseAndWritten(filesDataResponse);

            _filesInfoProject.ChangeFilesStatus(filesStatusWrite);

        }

        /// <summary>
        /// Отмена операции
        /// </summary>
        private async Task AbortConverting()
        {
            if (_statusProcessingInformation?.IsConverting == true)
            {
                await _fileConvertingServiceClient?.Operations?.AbortConvertingById(_filesInfoProject.Id);
            }           
        }

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        public void AbortPropertiesConverting()
        {
            IsIntermediateResponseInProgress = false;

            //не дожидаться завершения операции
            AbortConverting().ConfigureAwait(false);                                 

            ClearSubsriptions();
            _filesInfoProject?.ChangeAllFilesStatusAndMarkError();
        }

        /// <summary>
        /// Очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubsriptions()
        {
            _statusProcessingUpdaterSubsriptions?.Dispose();
        }
        #endregion

        public void Dispose()
        {
            AbortPropertiesConverting();
        }

    }
}
