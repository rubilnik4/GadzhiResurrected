using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDAL.Entities.Signatures;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiWcfHost.Services
{
    /// <summary>
    /// Сервис для получения и записи подписей. Контракт используется серверной частью
    /// </summary>   
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class SignatureServerService : ISignatureServerService
    {
        public SignatureServerService(ISignaturesService signaturesService)
        {
            _signaturesService = signaturesService;
        }

        /// <summary>
        /// Получение и запись из БД подписей и идентификаторов
        /// </summary>
        private readonly ISignaturesService _signaturesService;

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => 
            await _signaturesService.GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids) =>
            await _signaturesService.GetSignatures(ids);

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
