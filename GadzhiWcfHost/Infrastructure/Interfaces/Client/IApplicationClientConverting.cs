using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Interfaces.Client
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public interface IApplicationClientConverting
    {
        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        Task<FilesDataIntermediateResponseClient> QueueFilesDataAndGetResponse(FilesDataRequestClient filesDataRequest);

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        Task<FilesDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid filesDataServerID);

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        Task<FilesDataResponseClient> GetFilesDataResponseByID(Guid filesDataServerID);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task AbortConvertingById(Guid id);
    }
}