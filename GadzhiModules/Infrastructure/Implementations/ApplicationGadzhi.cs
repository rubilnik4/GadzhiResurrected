﻿using ChannelAdam.ServiceModel;
using GadzhiCommon.Helpers.Dialogs;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiCommon.Extensions.Functional;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Слой приложения, инфраструктура
    /// </summary>
    public sealed class ApplicationGadzhi : IApplicationGadzhi
    {
        /// <summary>
        /// Модель конвертируемых файлов
        /// </summary>     
        private readonly IPackageData _packageInfoProject;

        /// <summary>
        /// Сервис конвертации
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingClientService> _fileConvertingClientService;

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
        /// Получить информацию о состоянии конвертируемых файлов. Таймер с подпиской
        /// </summary>
        private readonly CompositeDisposable _statusProcessingSubscriptions;

        public ApplicationGadzhi(IDialogServiceStandard dialogServiceStandard,
                                 IFileSystemOperations fileSystemOperations,
                                 IPackageData packageInfoProject,
                                 IServiceConsumer<IFileConvertingClientService> fileConvertingClientService,
                                 IFileDataProcessingStatusMark fileDataProcessingStatusMark,
                                 IStatusProcessingInformation statusProcessingInformation)
        {
            _dialogServiceStandard = dialogServiceStandard ?? throw new ArgumentNullException(nameof(dialogServiceStandard));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _packageInfoProject = packageInfoProject ?? throw new ArgumentNullException(nameof(packageInfoProject));
            _fileConvertingClientService = fileConvertingClientService ?? throw new ArgumentNullException(nameof(fileConvertingClientService));
            _fileDataProcessingStatusMark = fileDataProcessingStatusMark ?? throw new ArgumentNullException(nameof(fileDataProcessingStatusMark));
            _statusProcessingInformation = statusProcessingInformation ?? throw new ArgumentNullException(nameof(statusProcessingInformation));

            _statusProcessingSubscriptions = new CompositeDisposable();
        }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange => _packageInfoProject.FileDataChange;

        /// <summary>
        /// Выполняется ли промежуточный запрос
        /// </summary>
        private bool IsIntermediateResponseInProgress { get; set; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        public async Task AddFromFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var filePaths = await _dialogServiceStandard.OpenFileDialog(true, DialogFilters.DocAndDgn);
            await AddFromFilesOrDirectories(filePaths);
        }

        /// <summary>
        /// Указать папку для конвертации
        /// </summary>     
        public async Task AddFromFolders()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var directoryPaths = await _dialogServiceStandard.OpenFolderDialog(true);
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

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication()
        {
            if (_statusProcessingInformation.IsConverting &&
                !_dialogServiceStandard.ShowMessageOkCancel("Бросить все на полпути?"))
            {
                return;
            }
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>
        public async Task ConvertingFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;

            var packageDataRequest = await PrepareFilesToSending();
            if (!packageDataRequest.IsValid)
            {
                await AbortPropertiesConverting();
                _dialogServiceStandard.ShowAndLogMessage("Необходимо загрузить файлы для конвертирования");
            }

            await SendFilesToConverting(packageDataRequest);
            SubscribeToIntermediateResponse();
        }

        /// <summary>
        /// Подготовить данные к отправке
        /// </summary>     
        private async Task<PackageDataRequestClient> PrepareFilesToSending()
        {
            var filesStatusInSending = await _fileDataProcessingStatusMark.GetFilesInSending();
            _packageInfoProject.ChangeFilesStatus(filesStatusInSending);

            var filesDataRequest = await _fileDataProcessingStatusMark.GetFilesDataToRequest();
            return filesDataRequest;
        }

        /// <summary>
        /// Отправить файлы для конвертации
        /// </summary>
        private async Task SendFilesToConverting(PackageDataRequestClient packageDataRequest)
        {
            var packageDataResponse = await _fileConvertingClientService.Operations.SendFiles(packageDataRequest);
            var filesStatusAfterSending = await _fileDataProcessingStatusMark.GetPackageStatusAfterSend(packageDataRequest, packageDataResponse);
            _packageInfoProject.ChangeFilesStatus(filesStatusAfterSending);
        }

        /// <summary>
        /// Подписаться на изменение статуса файлов при конвертировании
        /// </summary>
        private void SubscribeToIntermediateResponse() =>
            _statusProcessingSubscriptions.
            Add(Observable.
                Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToIntermediateResponse)).
                Where(_ => _statusProcessingInformation.IsConverting && !IsIntermediateResponseInProgress).
                Subscribe(async _ => await ExecuteAndHandleErrorAsync(UpdateStatusProcessing,
                                                                      () => IsIntermediateResponseInProgress = true,
                                                                      async () => await AbortPropertiesConverting(),
                                                                      () => IsIntermediateResponseInProgress = false)));

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task UpdateStatusProcessing()
        {
            PackageDataIntermediateResponseClient packageDataResponse =  await _fileConvertingClientService.Operations.
                                                                                CheckFilesStatusProcessing(_packageInfoProject.Id);
            PackageStatus packageStatus = await _fileDataProcessingStatusMark.GetPackageStatusIntermediateResponse(packageDataResponse);
            _packageInfoProject.ChangeFilesStatus(packageStatus);

            if (CheckStatusProcessing.CompletedStatusProcessingProject.Contains(packageDataResponse.StatusProcessingProject))
            {
                await GetCompleteFiles();
                ClearSubscriptions();
            }
        }

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        private async Task GetCompleteFiles()
        {
            var packageDataResponse = await _fileConvertingClientService.Operations.GetCompleteFiles(_packageInfoProject.Id);

            var filesStatusBeforeWrite = await _fileDataProcessingStatusMark.GetFilesStatusCompleteResponseBeforeWriting(packageDataResponse);
            _packageInfoProject.ChangeFilesStatus(filesStatusBeforeWrite);

            var filesStatusWrite = await _fileDataProcessingStatusMark.
                                         GetFilesStatusCompleteResponseAndWritten(packageDataResponse);
            _packageInfoProject.ChangeFilesStatus(filesStatusWrite);

            await _fileConvertingClientService.Operations.SetFilesDataLoadedByClient(_packageInfoProject.Id);
        }

        /// <summary>
        /// Отмена операции
        /// </summary>
        private async Task AbortConverting()
        {
            if (_statusProcessingInformation?.IsConverting == true && _packageInfoProject != null)
            {
                await _fileConvertingClientService.Operations.AbortConvertingById(_packageInfoProject.Id);
            }
        }

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        public async Task AbortPropertiesConverting(bool isDispose = false)
        {
            IsIntermediateResponseInProgress = false;

            await AbortConverting();
            if (isDispose)
            {
                _fileConvertingClientService?.Dispose();
            }

            ClearSubscriptions();
            _statusProcessingSubscriptions?.Dispose();
            _packageInfoProject?.ChangeAllFilesStatusAndMarkError();
        }

        /// <summary>
        /// Загрузить подписи из базы данных
        /// </summary>
        public async Task<IReadOnlyList<ISignatureLibrary>> GetSignaturesNames() =>
            await _fileConvertingClientService.Operations.GetSignaturesNames().
            MapAsync(ConverterDataFileFromDto.SignaturesLibraryFromDto).
            MapAsync(signatures => signatures.ToList());

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        public async Task<IList<string>> GetSignaturesDepartments() =>
            await _fileConvertingClientService.Operations.GetSignaturesDepartments();

        /// <summary>
        /// Очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubscriptions()
        {
            _statusProcessingSubscriptions?.Clear();
        }

        #region IDisposable Support
        private bool _disposedValue;

        void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {

            }
            AbortPropertiesConverting(true).ConfigureAwait(false);

            _disposedValue = true;
        }

        ~ApplicationGadzhi()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion      
    }
}
