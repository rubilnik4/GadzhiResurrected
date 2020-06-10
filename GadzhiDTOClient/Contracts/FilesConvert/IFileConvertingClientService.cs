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
        Task<PackageDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid filesDataId);

        /// <summary>
        /// Отправить отконвертированные файлы
        /// </summary>  
        [OperationContract]
        Task<PackageDataResponseClient> GetCompleteFiles(Guid filesDataId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary> 
        [OperationContract]
        Task SetFilesDataLoadedByClient(Guid filesDataId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>   
        [OperationContract]
        Task AbortConvertingById(Guid id);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>
        [OperationContract]
        Task<IList<SignatureDto>> GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>
        [OperationContract]
        Task<IList<DepartmentType>> GetSignaturesDepartments();
    }
}

