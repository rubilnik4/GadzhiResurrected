using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using System;
using System.Threading.Tasks;
using GadzhiDAL.Services.Interfaces;
using System.Collections.Generic;
using GadzhiDTOServer.TransferModels.Signatures;
using GadzhiDAL.Entities.Signatures;
using System.Linq;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiWcfHost.Infrastructure.Implementations.Logger;

namespace GadzhiWcfHost.Infrastructure.Implementations.Server
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public class ApplicationServerConverting : IApplicationServerConverting
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>
        private readonly IFilesDataServerService _filesDataServerService;

        /// <summary>
        /// Сервис для добавления и получения данных о подписях
        /// </summary>
        private readonly ISignaturesService _signaturesService;

        public ApplicationServerConverting(IFilesDataServerService filesDataServerService, ISignaturesService signaturesService)
        {
            _filesDataServerService = filesDataServerService ?? throw new ArgumentNullException(nameof(filesDataServerService));
            _signaturesService = signaturesService ?? throw new ArgumentNullException(nameof(signaturesService));
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>           
        public async Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName) =>
            await _filesDataServerService.GetFirstInQueuePackage(identityServerName).
            VoidAsync(package => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Download, ReflectionInfo.GetMethodBase(this),
                                                                  AuthLogging.GetParameterAuth(package?.Id.ToString() ?? "No Package")));

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse) =>
            await _filesDataServerService.UpdateFromIntermediateResponse(packageDataIntermediateResponse).
            VoidAsync(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Update, ReflectionInfo.GetMethodBase(this),
                                                            AuthLogging.GetParameterAuth(packageDataIntermediateResponse.Id.ToString())));

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task UpdateFromResponse(PackageDataResponseServer packageDataResponse) =>      
            await _filesDataServerService.UpdateFromResponse(packageDataResponse).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       AuthLogging.GetParameterAuth(packageDataResponse.Id.ToString())));

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion) =>
            await _filesDataServerService.DeleteAllUnusedPackagesUntilDate(dateDeletion).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Request, ReflectionInfo.GetMethodBase(this),
                                                       dateDeletion.ToShortDateString()));

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        public async Task DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion) =>
            await _filesDataServerService.DeleteAllUnusedErrorPackagesUntilDate(dateDeletion).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Request, ReflectionInfo.GetMethodBase(this),
                                                       dateDeletion.ToShortDateString()));

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id) => 
            await _filesDataServerService.AbortConvertingById(id).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Request, ReflectionInfo.GetMethodBase(this), id.ToString()));
        
        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() =>
            await _signaturesService.GetSignaturesNames().
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Request, ReflectionInfo.GetMethodBase(this), 
                                                       Authentication.Authentication.GetIdentityName()));

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids) =>
            await _signaturesService.GetSignatures(ids.Distinct().ToList()).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Request, ReflectionInfo.GetMethodBase(this),
                                                       Authentication.Authentication.GetIdentityName()));

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        public async Task UploadSignatures(IList<SignatureDto> signaturesDto) =>
            await _signaturesService.UploadSignatures(signaturesDto).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       Authentication.Authentication.GetIdentityName()));

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetSignaturesMicrostation ()=>
            await _signaturesService.GetMicrostationDataFile(MicrostationDataFiles.MICROSTATION_SIGNATURES_ID).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       Authentication.Authentication.GetIdentityName()));

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetStampsMicrostation() =>
            await _signaturesService.GetMicrostationDataFile(MicrostationDataFiles.MICROSTATION_STAMPS_ID).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       Authentication.Authentication.GetIdentityName()));

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        public async Task UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _signaturesService.UploadMicrostationDataFile(microstationDataFileDto, MicrostationDataFiles.MICROSTATION_SIGNATURES_ID).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       Authentication.Authentication.GetIdentityName()));

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        public async Task UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _signaturesService.UploadMicrostationDataFile(microstationDataFileDto, MicrostationDataFiles.MICROSTATION_STAMPS_ID).
            Void(_ => _loggerService.LogByObjectMethod(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this),
                                                       Authentication.Authentication.GetIdentityName()));
    }
}