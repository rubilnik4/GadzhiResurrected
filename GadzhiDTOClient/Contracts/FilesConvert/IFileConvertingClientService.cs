using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiDTOClient.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется клиентской частью
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IFileConvertingClientService
    {
        /// <summary>
        /// Отправить файлы для конвертирования
        /// </summary>
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        Task<PackageDataIntermediateResponseClient> SendFiles(PackageDataRequestClient packageDataRequestClient);

        /// <summary>
        /// Проверить статус файлов
        /// </summary>   
        [OperationContract(IsInitiating = false , IsTerminating = false)]
        Task<PackageDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid packageId);

        /// <summary>
        /// Отправить отконвертированные файлы
        /// </summary>  
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        Task<PackageDataResponseClient> GetCompleteFiles(Guid packageId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary> 
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        Task SetFilesDataLoadedByClient(Guid packageId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>   
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        Task AbortConvertingById(Guid packageId);
    }
}

