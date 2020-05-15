using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiDTOServer.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов.Контракт используется серверной частью
    /// </summary>
    [ServiceContract]
    public interface IFileConvertingServerService
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>    
        [OperationContract]
        Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        [OperationContract]
        Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>  
        [OperationContract]
        Task UpdateFromResponse(PackageDataResponseServer packageDataResponse);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary> 
        [OperationContract]
        Task DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>   
        [OperationContract]
        Task AbortConvertingById(Guid id);

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        [OperationContract]
        Task UploadSignatures(IList<SignatureDto> signaturesDto);

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        [OperationContract]
        Task UploadSignaturesMicrostation(SignatureMicrostationDto signaturesMicrostationDto);
    }
}

