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
using GadzhiCommon.Models.Implementations.Functional;
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
            await _filesDataServerService.GetFirstInQueuePackage(identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse) =>
            await _filesDataServerService.UpdateFromIntermediateResponse(packageDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>
        public async Task<Unit> UpdateFromResponse(PackageDataResponseServer packageDataResponse) =>
            await _filesDataServerService.UpdateFromResponse(packageDataResponse);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task<Unit> DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion) =>
            await _filesDataServerService.DeleteAllUnusedPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        public async Task<Unit> DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion) =>
            await _filesDataServerService.DeleteAllUnusedErrorPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task<Unit> AbortConvertingById(Guid id) =>
            await _filesDataServerService.AbortConvertingById(id);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() =>
            await _signaturesService.GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids) =>
            await _signaturesService.GetSignatures(ids.Distinct().ToList());

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        public async Task<Unit> UploadSignatures(IList<SignatureDto> signaturesDto) =>
            await _signaturesService.UploadSignatures(signaturesDto);

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetSignaturesMicrostation() =>
            await _signaturesService.GetMicrostationDataFile(MicrostationDataFiles.MICROSTATION_SIGNATURES_ID);

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetStampsMicrostation() =>
            await _signaturesService.GetMicrostationDataFile(MicrostationDataFiles.MICROSTATION_STAMPS_ID);

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        public async Task<Unit> UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _signaturesService.UploadMicrostationDataFile(microstationDataFileDto, MicrostationDataFiles.MICROSTATION_SIGNATURES_ID);

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        public async Task<Unit> UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _signaturesService.UploadMicrostationDataFile(microstationDataFileDto, MicrostationDataFiles.MICROSTATION_STAMPS_ID);
    }
}