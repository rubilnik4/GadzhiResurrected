using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;

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
        /// <summary>
        /// Класс для отправки пакетов на сервер
        /// </summary>
        private readonly IApplicationServerConverting _applicationServerConverting;

        public SignatureServerService(IApplicationServerConverting applicationServerConverting)
        {
            _applicationServerConverting = applicationServerConverting;
        }

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignaturesNames() => await _applicationServerConverting.GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        public async Task<IList<SignatureDto>> GetSignatures(IList<string> ids) => await _applicationServerConverting.GetSignatures(ids);

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        public async Task<Unit> UploadSignatures(IList<SignatureDto> signaturesDto) => await _applicationServerConverting.UploadSignatures(signaturesDto);

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetSignaturesMicrostation() => await _applicationServerConverting.GetSignaturesMicrostation();

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>   
        public async Task<MicrostationDataFileDto> GetStampsMicrostation() => await _applicationServerConverting.GetStampsMicrostation();

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        public async Task<Unit> UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _applicationServerConverting.UploadSignaturesMicrostation(microstationDataFileDto);

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        public async Task<Unit> UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto) =>
            await _applicationServerConverting.UploadStampsMicrostation(microstationDataFileDto);
    }
}
