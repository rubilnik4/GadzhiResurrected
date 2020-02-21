using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для получения файлов, у которых необходимо изменить статус
    /// </summary>
    public interface IFileDataProcessingStatusMark
    {
        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        Task<FilesDataRequestClient> GetFilesDataToRequest();

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        Task<FilesStatus> GetFilesInSending();

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        Task<FilesStatus> GetFilesNotFound(IEnumerable<FileDataRequestClient> fileDataRequest);

        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        Task<FilesStatus> GetFilesStatusIntermediateResponse(FilesDataIntermediateResponseClient fileDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        Task<FilesStatus> GetFilesStatusCompleteResponseBeforeWriting(FilesDataResponseClient filesDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета
        /// </summary>       
        Task<FilesStatus> GetFilesStatusCompleteResponseAndWritten(FilesDataResponseClient filesDataResponse);

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        Task<FilesStatus> GetFilesStatusUnionAfterSendAndNotFound(FilesDataRequestClient filesDataRequest,
                                                                  FilesDataIntermediateResponseClient filesDataIntermediateResponse);
    }
}
