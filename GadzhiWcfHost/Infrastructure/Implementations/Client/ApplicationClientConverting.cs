using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using System;
using System.Threading.Tasks;
using GadzhiDAL.Services.Interfaces;
using System.Collections;
using System.Collections.Generic;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiWcfHost.Infrastructure.Implementations.Client.Logger;

namespace GadzhiWcfHost.Infrastructure.Implementations.Client
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationClientConverting : IApplicationClientConverting
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в клиентской части
        /// </summary>
        private readonly IFilesDataClientService _filesDataClientService;

        /// <summary>
        /// Сервис для добавления и получения данных о подписях
        /// </summary>
        private readonly ISignaturesService _signaturesService;

        public ApplicationClientConverting(IFilesDataClientService filesDataClientService, ISignaturesService signaturesService)
        {
            _filesDataClientService = filesDataClientService ?? throw new ArgumentNullException(nameof(filesDataClientService));
            _signaturesService = signaturesService ?? throw new ArgumentNullException(nameof(signaturesService));
        }

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        public async Task<PackageDataIntermediateResponseClient> QueueFilesDataAndGetResponse(PackageDataRequestClient packageDataRequest)
        {
            _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                             AuthLogging.GetParameterAuth(packageDataRequest?.Id));
            if (packageDataRequest == null) return new PackageDataIntermediateResponseClient();

            await QueueFilesData(packageDataRequest, Authentication.GetIdentityName());
            return await GetIntermediateFilesDataResponseById(packageDataRequest.Id);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName) =>
            await _filesDataClientService.QueueFilesData(packageDataRequest, identityName).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       AuthLogging.GetParameterAuth(packageDataRequest.Id)));

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по Id номеру
        /// </summary>
        public async Task<PackageDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid packageId) =>
            await _filesDataClientService.GetFilesDataIntermediateResponseById(packageId);

        /// <summary>
        /// Получить отконвертированные файлы по Id номеру
        /// </summary>
        public async Task<PackageDataResponseClient> GetFilesDataResponseById(Guid packageId) =>
            await _filesDataClientService.GetFilesDataResponseById(packageId).
            VoidAsync(package => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Download,
                                                            ReflectionInfo.GetMethodBase(this), AuthLogging.GetParameterAuth(packageId)));

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid packageId) =>
            await _filesDataClientService.SetFilesDataLoadedByClient(packageId).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Request, 
                                                       ReflectionInfo.GetMethodBase(this), AuthLogging.GetParameterAuth(packageId)));

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid packageId) =>
            await _filesDataClientService.AbortConvertingById(packageId).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Abort, 
                                                 ReflectionInfo.GetMethodBase(this), AuthLogging.GetParameterAuth(packageId)));

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() =>
            await _signaturesService.GetSignaturesNames().
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Response, 
                                                       ReflectionInfo.GetMethodBase(this), Authentication.GetIdentityName()));

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>  
        public async Task<IList<DepartmentType>> GetSignaturesDepartments() =>
            await _signaturesService.GetSignaturesDepartments().
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Response, 
                                                       ReflectionInfo.GetMethodBase(this), Authentication.GetIdentityName()));
    }
}