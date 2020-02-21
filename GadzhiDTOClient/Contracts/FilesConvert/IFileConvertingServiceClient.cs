using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiDTOClient.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов.Контракт используется клиентской частью
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IFileConvertingClientService
    {
        /// <summary>
        /// Отправить файлы для конвертирования
        /// </summary>
        [OperationContract(IsInitiating = true,
                           IsTerminating = false)]
        Task<FilesDataIntermediateResponseClient> SendFiles(FilesDataRequestClient filesDataRequestClient);

        /// <summary>
        /// Проверить статус файлов
        /// </summary>   
        [OperationContract(IsInitiating = false,
                           IsTerminating = false)]
        Task<FilesDataIntermediateResponseClient> CheckFilesStatusProcessing(Guid filesDataID);

        /// <summary>
        /// Отправить отконвертированные файлы
        /// </summary>  
        [OperationContract(IsInitiating = false,
                           IsTerminating = true)]
        Task<FilesDataResponseClient> GetCompleteFiles(Guid filesDataID);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>   
        [OperationContract(IsInitiating = false,
                          IsTerminating = true)]
        Task AbortConvertingById(Guid id);
    }
}

