using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOClient.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов. Контракт используется клиентской частью
    /// </summary>
    [ServiceContract]
    public interface IFileConvertingClientService
    {
        /// <summary>
        /// Отправить файлы для конвертирования
        /// </summary>
        [OperationContract]
        Task<PackageDataIntermediateResponseClient> SendFiles(PackageDataRequestClient packageDataRequestClient);

        /// <summary>
        /// Проверить статус файлов
        /// </summary>   
        [OperationContract]
        Task<PackageDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid packageId);

        /// <summary>
        /// Отправить отконвертированные файлы
        /// </summary>  
        [OperationContract]
        Task<PackageDataResponseClient> GetCompleteFiles(Guid packageId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary> 
        [OperationContract]
        Task SetFilesDataLoadedByClient(Guid packageId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>   
        [OperationContract]
        Task AbortConvertingById(Guid packageId);
    }
}

