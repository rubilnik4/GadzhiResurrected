using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public interface IApplicationReceiveAndSend
    {
        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest);        
    }
}