using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using Nito.AsyncEx.Synchronous;
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
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

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

            var packageDataRequest = await PrepareFilesToSending();
            if (!packageDataRequest.IsValid)
            {
                await AbortPropertiesConverting(false);
                await _dialogService.ShowMessage("Загрузите файлы для конвертирования");
                return;
            }

            SubscribeToIntermediateResponse();
            //var sendResult = await SendFilesToConverting(packageDataRequest);
            //if (sendResult.OkStatus) SubscribeToIntermediateResponse();
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
            await _wcfServiceFactory.UsingConvertingService(service => service.Operations.SendFiles(packageDataRequest)).
            WhereContinueAsync(packageResult => packageResult.OkStatus,
                okFunc: packageResult => SendFilesToConvertingConnect(packageDataRequest, packageResult.Value),
                badFunc: packageResult => packageResult.
                                          ResultVoidAsyncBind(_ => AbortPropertiesConverting(false)).
                                          ResultVoidAsyncBind(_ => _dialogService.ShowErrors(packageResult.Errors)).
                                          MapAsync(package => package.ToResult()));

        /// <summary>
        /// Отправить файлы для конвертации после подтверждения сервера
        /// </summary>
        private async Task<IResultError> SendFilesToConvertingConnect(PackageDataRequestClient packageDataRequest, PackageDataIntermediateResponseClient packageDataResponse) =>
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
                                                                                  () => IsIntermediateResponseInProgress = true,
                                                                                  () => AbortPropertiesConverting(true).WaitAndUnwrapException(),
                                                                                  () => IsIntermediateResponseInProgress = false).
                                                       ResultVoidBadAsync(errors => _dialogService.ShowErrors(errors)))).
                Concat().
                Subscribe());

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task<IResultError> UpdateStatusProcessing() =>
            await _wcfServiceFactory.UsingConvertingService(service => service.Operations.CheckFilesStatusProcessing(_packageData.Id)).
            ResultValueOkAsync(packageDataResponse => _fileDataProcessingStatusMark.GetPackageStatusIntermediateResponse(packageDataResponse)).
            ResultVoidOkAsync(packageStatus => _packageData.ChangeFilesStatus(packageStatus)).
            ResultValueOkAsync(packageStatus => packageStatus.StatusProcessingProject.
                                                WhereOkAsyncBind(statusProject => CheckStatusProcessing.CompletedStatusProcessingProject.Contains(statusProject),
                                                    okFunc: statusProject => statusProject.
                                                                             VoidBindAsync(_ => GetCompleteFiles()).
                                                                             VoidAsync(_ => ClearSubscriptions()))).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        private async Task GetCompleteFiles() =>
            await _wcfServiceFactory.UsingConvertingService(service => service.Operations.GetCompleteFiles(_packageData.Id)).
            ResultVoidBadAsync(errors => _dialogService.ShowErrors(errors)).
            ResultVoidOkAsync(packageDataResponse => _loggerService.LogByObjects(LoggerLevel.Info, LoggerAction.Download, ReflectionInfo.GetMethodBase(this),
                                                                                 packageDataResponse.FilesData, packageDataResponse.Id.ToString())).
            ResultValueOkAsync(packageDataResponse => _fileDataProcessingStatusMark.GetFilesStatusCompleteResponseBeforeWriting(packageDataResponse).
                                                      VoidAsync(filesStatusBeforeWrite => _packageData.ChangeFilesStatus(filesStatusBeforeWrite)).
                                                      MapBindAsync(_ => _fileDataProcessingStatusMark.GetFilesStatusCompleteResponseAndWritten(packageDataResponse)).
                                                      VoidAsync(filesStatusWrite => _packageData.ChangeFilesStatus(filesStatusWrite))).
            ResultVoidOkAsync(_ => _wcfServiceFactory.UsingConvertingService(service => service.Operations.SetFilesDataLoadedByClient(_packageData.Id))).
            ResultVoidBadAsync(errors => _dialogService.ShowErrors(errors));

        /// <summary>
        /// Отмена операции
        /// </summary>
        private async Task AbortConverting() =>
            await _statusProcessingInformation.
            WhereOkAsyncBind(status => status?.IsConverting == true,
                okFunc: status => status.
                        VoidBindAsync(_ => _wcfServiceFactory.UsingConvertingService(service => service.Operations.AbortConvertingById(_packageData.Id)).
                                           MapAsync(result => result.ToResultValue(Unit.Value)).
                                           ResultVoidBadAsync(errors => _dialogService.ShowErrors(errors))));

        /// <summary>
        /// Загрузить подписи из базы данных
        /// </summary>
        [Logger]
        public async Task<IResultCollection<ISignatureLibrary>> GetSignaturesNames() =>
            await _wcfServiceFactory.UsingSignatureService((service) => service.Operations.GetSignaturesNames().
                                                                        MapAsync(ConverterDataFileFromDto.SignaturesLibraryFromDto)).
            MapAsync(result => result.ToResultCollection());

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        [Logger]
        public async Task<IResultCollection<DepartmentType>> GetSignaturesDepartments() =>
            await _wcfServiceFactory.UsingSignatureService((service) => service.Operations.GetSignaturesDepartments()).
            MapAsync(result => result.ToResultCollection());

        /// <summary>
        /// Сбросить индикаторы конвертации
        /// </summary>
        public async Task AbortPropertiesConverting(bool abortConnection)
        {
            IsIntermediateResponseInProgress = false;

            if (abortConnection) await AbortConverting();

            if (_statusProcessingInformation?.IsConverting == true)
            {
                _packageData?.ChangeAllFilesStatusAndMarkError();
            }
            ClearSubscriptions();
        }

        /// <summary>
        /// Очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubscriptions()
        {
            _statusProcessingSubscriptions?.Clear();
            _wcfServiceFactory?.DisposeServices();
        }
    }
}