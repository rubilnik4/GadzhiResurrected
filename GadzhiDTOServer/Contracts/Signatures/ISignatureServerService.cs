using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiDTOServer.Contracts.Signatures
{
    /// <summary>
    /// Сервис для получения и записи подписей.Контракт используется серверной частью
    /// </summary>
    [ServiceContract]
    public interface ISignatureServerService
    {
        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>
        [OperationContract]
        Task<IList<SignatureDto>> GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>
        [OperationContract]
        Task<IList<SignatureDto>> GetSignatures(IList<string> ids);

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        [OperationContract]
        Task<Unit> UploadSignatures(IList<SignatureDto> signaturesDto);

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>
        [OperationContract]
        Task<MicrostationDataFileDto> GetStampsMicrostation();

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>
        [OperationContract]
        Task<MicrostationDataFileDto> GetSignaturesMicrostation();

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        [OperationContract]
        Task<Unit> UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto);

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        [OperationContract]
        Task<Unit> UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto);
    }
}