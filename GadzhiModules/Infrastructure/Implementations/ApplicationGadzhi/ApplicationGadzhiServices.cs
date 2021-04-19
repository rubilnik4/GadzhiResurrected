using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Infrastructure.Implementations.Validates;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiModules.Infrastructure.Implementations.ApplicationGadzhi
{
    /// <summary>
    /// Слой приложения, инфраструктура. Сервисы
    /// </summary>
    public partial class ApplicationGadzhi
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Выполняется ли промежуточный запрос
        /// </summary>
        private bool IsIntermediateResponseInProgress { get; set; }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>
        [Logger]
        public async Task ConvertingFiles()
        {
            if (_statusProcessingInformation.IsConverting) return;
            
            var filesPath = _packageData.FilesData.Select(fileData => fileData.FilePath).ToList();
            var resultError = FilesDataValidation.ValidateFilesData(filesPath, _fileSystemOperations);
            if (resultError.HasErrors)
            {
                await AbortPropertiesConverting(false, resultError.Errors.First());
                await _dialogService.ShowError(resultError.Errors.First());
                return;
            }

            var packageDataRequest = await PrepareFilesToSending();
            var sendResult = await SendFilesToConverting(packageDataRequest);
            if (sendResult.OkStatus) SubscribeToIntermediateResponse();
        }

        /// <summary>
        /// Подготовить данные к отправке
        /// </summary>
        private async Task<PackageDataRequestClient> PrepareFilesToSending()
        {
            var filesStatusInSending = await _fileDataProcessingStatusMark.GetFilesInSending();
            _packageData.ChangeFilesStatus(filesStatusInSending);

            var filesDataRequest = await _fileDataProcessingStatusMark.GetFilesDataToRequest();
            return filesDataRequest;
        }

        /// <summary>
        /// Отправить файлы для конвертации
        /// </summary>
        private async Task<IResultError> SendFilesToConverting(PackageDataRequestClient packageDataRequest) =>
            await _wcfClientServiceFactory.ConvertingClientServiceFactory.UsingService(service => service.Operations.SendFiles(packageDataRequest)).
            WhereContinueAsyncBind(packageResult => packageResult.OkStatus,
                okFunc: packageResult => SendFilesToConvertingConnect(packageDataRequest, packageResult.Value),
                badFunc: packageResult => packageResult.
                                          ResultVoidAsyncBind(_ => AbortPropertiesCommunication()).
                                          MapAsync(package => package.ToResult()));

        /// <summary>
        /// Отправить файлы для конвертации после подтверждения сервера
        /// </summary>
        private async Task<IResultError> SendFilesToConvertingConnect(PackageDataRequestClient packageDataRequest,
                                                                      PackageDataShortResponseClient packageDataResponse) =>
            await packageDataResponse.
            Void(_ => _loggerService.LogByObjects(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                  packageDataRequest.FilesData, packageDataRequest.Id.ToString())).
            Map(_ => _fileDataProcessingStatusMark.GetPackageStatusAfterSend(packageDataRequest, packageDataResponse)).
            VoidAsync(filesStatusAfterSending => _packageData.ChangeFilesStatus(filesStatusAfterSending)).
            MapAsync(_ => new ResultError());

        /// <summary>
        /// Подписаться на изменение статуса файлов при конвертировании
        /// </summary>
        private void SubscribeToIntermediateResponse() =>
            _statusProcessingSubscriptions.
            Add(Observable.
                Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToIntermediateResponse)).
                Where(_ => _statusProcessingInformation.IsConverting && !IsIntermediateResponseInProgress).
                Select(_ => Observable.FromAsync(() => ExecuteAndHandleErrorAsync(UpdateStatusProcessing,
                                                                                  beforeMethod: () => IsIntermediateResponseInProgress = true,
                                                                                  finallyMethod: () => IsIntermediateResponseInProgress = false))).
                Concat().
                Subscribe());

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task<IResultError> UpdateStatusProcessing() =>
            await _wcfClientServiceFactory.ConvertingClientServiceFactory.
            UsingServiceRetry(service => service.Operations.CheckFilesStatusProcessing(_packageData.Id)).
            ResultVoidBadBindAsync(_ => AbortPropertiesCommunication()).
            ResultValueOkAsync(packageDataResponse => _fileDataProcessingStatusMark.GetPackageStatusIntermediateResponse(packageDataResponse)).
            ResultValueOkBindAsync(DownloadFilesData).
            ResultValueOkAsync(packageStatus => packageStatus.StatusProcessingProject.
                                                WhereOkAsyncBind(statusProject => CheckStatusProcessing.CompletedStatusProcessingProject.Contains(statusProject),
                                                    okFunc: statusProject => statusProject.
                                                                             VoidBindAsync(_ => MarkFilesComplete(packageStatus)).
                                                                             VoidAsync(_ => ClearSubscriptions()))).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Загрузить файлы
        /// </summary>
        private async Task<IResultValue<PackageStatus>> DownloadFilesData(PackageStatus packageStatus)
        {
            var filesPath = _packageData.GetFilesToDownload(packageStatus);
            var completeFilesTasks = filesPath.Select(GetCompleteFile);
            var completeFiles = await Task.WhenAll(completeFilesTasks);
            var errors = completeFiles.SelectMany(completeFile => completeFile.Errors.ToList());
            return new ResultValue<PackageStatus>(packageStatus, errors);
        }

        /// <summary>
        /// Получить отконвертированный файл
        /// </summary>
        private async Task<IResultError> GetCompleteFile(string filePath) =>
            await _wcfClientServiceFactory.ConvertingClientServiceFactory.
            UsingServiceRetry(service => service.Operations.GetCompleteFile(_packageData.Id, filePath)).
            ResultVoidOkAsync(fileDataResponse => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Download, ReflectionInfo.GetMethodBase(this),
                                                                              fileDataResponse, _packageData.Id.ToString())).
            ResultValueOkAsync(fileDataResponse => _fileDataProcessingStatusMark.GetFileStatusCompleteResponseBeforeWriting(fileDataResponse).
                                                   Void(fileStatusBeforeWrite => _packageData.ChangeFileStatus(fileStatusBeforeWrite)).
                                                   Map(_ => _fileDataProcessingStatusMark.GetFileStatusCompleteResponseAndWritten(fileDataResponse)).
                                                   VoidAsync(filesStatusWrite => _packageData.ChangeFileStatus(filesStatusWrite))).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Отметить файлы как отконвертированные
        /// </summary>
        private async Task MarkFilesComplete(PackageStatus packageStatus) =>
            await new ResultValue<PackageStatus>(new PackageStatus(packageStatus.FileStatus.Select(fileStatus => fileStatus.GetWithStatusProcessing(StatusProcessing.End)),
                                                                   StatusProcessingProject.End)).
            ResultVoidOk(status => _packageData.ChangeFilesStatus(status)).
            ResultVoidOk(status => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Download, ReflectionInfo.GetMethodBase(this), _packageData.Id.ToString())).
            ResultVoidAsyncBind(_ => _wcfClientServiceFactory.ConvertingClientServiceFactory.UsingService(service => service.Operations.SetFilesDataLoadedByClient(_packageData.Id))).
            ResultVoidBadBindAsync(_ => AbortPropertiesCommunication());

        /// <summary>
        /// Отмена операции
        /// </summary>
        private async Task AbortConverting() =>
            await _statusProcessingInformation.
            WhereOkAsyncBind(status => status?.IsConverting == true,
                okFunc: status => status.
                        VoidBindAsync(_ => _wcfClientServiceFactory.ConvertingClientServiceFactory.
                                           UsingService(service => service.Operations.AbortConvertingById(_packageData.Id))));

        /// <summary>
        /// Загрузить подписи из базы данных
        /// </summary>
        [Logger]
        public async Task<IResultCollection<ISignatureLibrary>> GetSignaturesNames() =>
            await _wcfClientServiceFactory.SignatureClientServiceFactory.UsingService(service => service.Operations.GetSignaturesNames().
                                                                                                 MapAsync(ConverterDataFileFromDto.SignaturesLibraryFromDto)).
            MapAsync(result => result.ToResultCollection());

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        [Logger]
        public async Task<IResultCollection<DepartmentType>> GetSignaturesDepartments() =>
            await _wcfClientServiceFactory.SignatureClientServiceFactory.UsingService(service => service.Operations.GetSignaturesDepartments()).
            MapAsync(result => result.ToResultCollection());

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        public async Task AbortPropertiesConverting(bool abortConnection, IErrorCommon errorStatus)
        {
            IsIntermediateResponseInProgress = false;

            if (abortConnection) await AbortConverting();
            if (_statusProcessingInformation?.IsConverting == true) _packageData?.ChangeAllFilesStatusAndSetError(errorStatus);

            ClearSubscriptions();
        }

        /// <summary>
        /// Сбросить индикаторы конвертации при отмене конвертирования
        /// </summary>
        public async Task AbortPropertiesCancellation() =>
            await AbortPropertiesConverting(true, new ErrorCommon(ErrorConvertingType.AbortOperation, "Отмена конвертирования"));

        /// <summary>
        /// Сбросить индикаторы конвертации при ошибке соединения с сервером
        /// </summary>
        private async Task AbortPropertiesCommunication() =>
            await AbortPropertiesConverting(false, new ErrorCommon(ErrorConvertingType.Communication, "Связь с сервером не установлена"));

        /// <summary>
        /// Очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubscriptions()
        {
            _statusProcessingSubscriptions?.Clear();
            _wcfClientServiceFactory?.Dispose();
        }
    }
}