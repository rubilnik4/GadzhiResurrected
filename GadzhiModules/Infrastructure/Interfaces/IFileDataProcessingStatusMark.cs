using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Task<FilesDataRequest> GetFilesDataToRequest();

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        Task<FilesStatus> GetFilesInSending();

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        Task<FilesStatus> GetFilesNotFound(IEnumerable<FileDataRequest> fileDataRequest);

        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        Task<FilesStatus> GetFilesStatusIntermediateResponse(FilesDataIntermediateResponse fileDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        Task<FilesStatus> GetFilesStatusCompleteResponseBeforeWriting(FilesDataResponse filesDataResponse);       

        /// <summary>
        /// Поменять статус файлов после окончательного отчета
        /// </summary>       
        Task<FilesStatus> GetFilesStatusCompleteResponseAndWritten(FilesDataResponse filesDataResponse);
       
        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        Task<FilesStatus> GetFilesStatusUnionAfterSendAndNotFound(FilesDataRequest filesDataRequest,
                                                                  FilesDataIntermediateResponse filesDataIntermediateResponse);        
    }
}
