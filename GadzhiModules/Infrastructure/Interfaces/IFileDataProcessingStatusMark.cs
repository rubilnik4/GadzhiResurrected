﻿using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
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
        Task<IEnumerable<FileStatus>> GetFilesInSending();

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        Task<IEnumerable<FileStatus>> GetFilesNotFound(IEnumerable<FileDataRequest> fileDataRequest);


        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        Task<IEnumerable<FileStatus>> GetFilesStatusIntermediateResponse(FilesDataIntermediateResponse fileDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета
        /// </summary>       
        Task<IEnumerable<FileStatus>> GetFilesStatusCompliteResponse(FilesDataResponse filesDataResponse);
       
        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        Task<IEnumerable<FileStatus>> GetFilesStatusUnionAfterSendAndNotFound(FilesDataRequest filesDataRequest,
                                                                          FilesDataIntermediateResponse filesDataIntermediateResponse);        
    }
}
