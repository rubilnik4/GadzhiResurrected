using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.Signatures;
using GadzhiDTOServer.TransferModels.Signatures;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiWcfHost.Infrastructure.Interfaces.Server
{
    /// <summary>
    /// Класс для отправки пакетов на сервер
    /// </summary>
    public interface IApplicationServerConverting
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>          
        Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary> 
        Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>        
        Task<Unit> UpdateFromResponse(PackageDataResponseServer packageDataResponse);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        Task<Unit> DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion);

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        Task<Unit> DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>         
        Task<Unit> AbortConvertingById(Guid id);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        Task<IList<SignatureDto>> GetSignaturesNames();

        /// <summary>
        /// Загрузить подписи из базы данных по идентификаторам
        /// </summary>      
        Task<IList<SignatureDto>> GetSignatures(IList<string> ids);

        /// <summary>
        /// Загрузить подписи
        /// </summary>
        Task UploadSignatures(IList<SignatureDto> signaturesDto);

        /// <summary>
        /// Получить подписи Microstation из базы данных
        /// </summary>   
        Task<MicrostationDataFileDto> GetSignaturesMicrostation();

        /// <summary>
        /// Получить штампы Microstation из базы данных
        /// </summary>   
        Task<MicrostationDataFileDto> GetStampsMicrostation() ;

        /// <summary>
        /// Загрузить подписи Microstation
        /// </summary>
        Task UploadSignaturesMicrostation(MicrostationDataFileDto microstationDataFileDto);

        /// <summary>
        /// Загрузить штампы Microstation
        /// </summary>
        Task UploadStampsMicrostation(MicrostationDataFileDto microstationDataFileDto);
    }
}