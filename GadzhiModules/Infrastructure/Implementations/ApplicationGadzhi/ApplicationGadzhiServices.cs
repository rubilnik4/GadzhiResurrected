using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
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
                await AbortPropertiesConverting();
                await _dialogService.ShowMessage("Загрузите файлы для конвертирования");
                return;
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
            _loggerService.LogByObjects(LoggerLevel.Info, LoggerObjectAction.Upload, ReflectionInfo.GetMethodBase(this),
                                        packageDataRequest.FilesData, packageDataRequest.Id.ToString());

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
                Select(_ => Observable.FromAsync(() => ExecuteAndHandleErrorAsync(UpdateStatusProcessing,
                                                                                  () => IsIntermediateResponseInProgress = true,
                                                                                  () => AbortPropertiesConverting().WaitAndUnwrapException(),
                                                                                  () => IsIntermediateResponseInProgress = false))).
                Concat().
                Subscribe());

        /// <summary>
        /// Получить информацию о состоянии конвертируемых файлов
        /// </summary>
        private async Task UpdateStatusProcessing()
        {
            var packageDataResponse = await _fileConvertingClientService.Operations.CheckFilesStatusProcessing(_packageInfoProject.Id);
            var packageStatus = await _fileDataProcessingStatusMark.GetPackageStatusIntermediateResponse(packageDataResponse);
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
            _loggerService.LogByObjects(LoggerLevel.Info, LoggerObjectAction.Download, ReflectionInfo.GetMethodBase(this),
                                        packageDataResponse.FilesData, packageDataResponse.Id.ToString());

            var filesStatusBeforeWrite = await _fileDataProcessingStatusMark.GetFilesStatusCompleteResponseBeforeWriting(packageDataResponse);
            _packageInfoProject.ChangeFilesStatus(filesStatusBeforeWrite);

            var filesStatusWrite = await _fileDataProcessingStatusMark.GetFilesStatusCompleteResponseAndWritten(packageDataResponse);
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
                _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Abort, ReflectionInfo.GetMethodBase(this), _packageInfoProject.Id.ToString());
            }
        }

        /// <summary>
        /// Загрузить подписи из базы данных
        /// </summary>
        [Logger]
        public async Task<IReadOnlyList<ISignatureLibrary>> GetSignaturesNames() =>
            await _fileConvertingClientService.Operations.GetSignaturesNames().
            MapAsync(ConverterDataFileFromDto.SignaturesLibraryFromDto).
            MapAsync(signatures => signatures.ToList());

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        [Logger]
        public async Task<IList<DepartmentType>> GetSignaturesDepartments() =>
            await _fileConvertingClientService.Operations.GetSignaturesDepartments();

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

            if (_statusProcessingInformation?.IsConverting == true)
            {
                _packageInfoProject?.ChangeAllFilesStatusAndMarkError();
            }
            ClearSubscriptions();
        }

        /// <summary>
        /// Очистить подписки на обновление пакета конвертирования
        /// </summary>
        private void ClearSubscriptions() => _statusProcessingSubscriptions?.Clear();
    }
}