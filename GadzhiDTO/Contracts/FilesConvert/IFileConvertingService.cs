using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.Contracts.FilesConvert
{
    /// <summary>
    /// Сервис для конвертирования файлов.Контракт используется и клиентской и серверной частью
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IFileConvertingService
    {
        /// <summary>
        /// Отправить файлы для конвертирования
        /// </summary>
        [OperationContract(IsInitiating = true,
                           IsTerminating = false)]
        Task<FilesDataIntermediateResponse> SendFiles(FilesDataRequest filesDataRequest);

        /// <summary>
        /// Проверить статус файлов
        /// </summary>   
        [OperationContract(IsInitiating = false,
                           IsTerminating = false)]
        Task<FilesDataIntermediateResponse> CheckFilesStatusProcessing(Guid filesDataID);

        /// <summary>
        /// Отправить отконвертированные файлы
        /// </summary>  
        [OperationContract(IsInitiating = false,
                           IsTerminating = true)]
        Task<FilesDataResponse> GetCompleteFiles(Guid filesDataID);
    }
}

