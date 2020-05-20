using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Services.Implementations;
using GadzhiDTOServer.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using System;
using System.Threading.Tasks;
using GadzhiDAL.Services.Interfaces;
using System.Collections.Generic;
using GadzhiDTOServer.TransferModels.Signatures;
using GadzhiDAL.Entities.Signatures;
using System.Linq;

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
        private readonly ISignaturesServerService _signaturesServerService;

        public ApplicationServerConverting(IFilesDataServerService filesDataServerService, ISignaturesServerService signaturesServerService)
        {
            _filesDataServerService = filesDataServerService ?? throw new ArgumentNullException(nameof(filesDataServerService));
            _signaturesServerService = signaturesServerService ?? throw new ArgumentNullException(nameof(signaturesServerService));
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
        public async Task UpdateFromResponse(PackageDataResponseServer packageDataResponse) =>      
                await _filesDataServerService.UpdateFromResponse(packageDataResponse);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion) =>
                await _filesDataServerService.DeleteAllUnusedPackagesUntilDate(dateDeletion);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>
        public async Task AbortConvertingById(Guid id) => await _filesDataServerService.AbortConvertingById(id);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() =>
            await _signaturesServerService.GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids) =>
            await _signaturesServerService.GetSignatures(ids.Distinct().ToList());

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        public async Task UploadSignatures(IList<SignatureDto> signaturesDto) =>
            await _signaturesServerService.UploadSignatures(signaturesDto);

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetSignaturesMicrostation ()=>
            await _signaturesServerService.GetMicrostationDataFile(MicrostationDataFiles.MICROSTATION_SIGNATURES_ID);

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetStampsMicrostation() =>
            await _signaturesServerService.GetMicrostationDataFile(MicrostationDataFiles.MICROSTATION_STAMPS_ID);

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        public async Task UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _signaturesServerService.UploadMicrostationDataFile(microstationDataFileDto, MicrostationDataFiles.MICROSTATION_SIGNATURES_ID);

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        public async Task UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _signaturesServerService.UploadMicrostationDataFile(microstationDataFileDto, MicrostationDataFiles.MICROSTATION_STAMPS_ID);
    }
}