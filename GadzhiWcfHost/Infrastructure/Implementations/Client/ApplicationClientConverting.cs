using GadzhiDAL.Services.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using Microsoft.VisualStudio.Threading;
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
            _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Upload, ReflectionInfo.GetMethodBase(this),
                                       packageDataRequest?.Id.ToString() ?? "NullPackage");
            if (packageDataRequest == null) return new PackageDataIntermediateResponseClient();

            await QueueFilesData(packageDataRequest, Authentication.GetIdentityName());
            return await GetIntermediateFilesDataResponseById(packageDataRequest.Id);
        }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        private async Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName) =>      
            await _filesDataClientService.QueueFilesData(packageDataRequest, identityName).
            Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                 packageDataRequest.Id.ToString()));

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по Id номеру
        /// </summary>
        public async Task<PackageDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid filesDataServerId) =>      
            await _filesDataClientService.GetFilesDataIntermediateResponseById(filesDataServerId);       

        /// <summary>
        /// Получить отконвертированные файлы по Id номеру
        /// </summary>
        public async Task<PackageDataResponseClient> GetFilesDataResponseById(Guid filesDataServerId) =>
            await _filesDataClientService.GetFilesDataResponseById(filesDataServerId).
            VoidAsync(package => _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Download,
                                                            ReflectionInfo.GetMethodBase(this), package.Id.ToString()));

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid filesDataId) =>
            await _filesDataClientService.SetFilesDataLoadedByClient(filesDataId).
            Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Request, ReflectionInfo.GetMethodBase(this), filesDataId.ToString()));

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id) => 
            await _filesDataClientService.AbortConvertingById(id).
            Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Abort, ReflectionInfo.GetMethodBase(this), id.ToString()));

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => 
            await _signaturesService.GetSignaturesNames().
            Void(_ => _loggerService.LogByObject<string>(LoggerLevel.Info, LoggerObjectAction.Response, ReflectionInfo.GetMethodBase(this)));

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>  
        public async Task<IList<DepartmentType>> GetSignaturesDepartments() => 
            await _signaturesService.GetSignaturesDepartments().
            Void(_ => _loggerService.LogByObject<string>(LoggerLevel.Info, LoggerObjectAction.Response, ReflectionInfo.GetMethodBase(this)));
    }
}