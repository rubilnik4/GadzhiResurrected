using GadzhiDTOClient.TransferModels.FilesConvert;
using Microsoft.VisualStudio.Threading;
using System;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Interfaces.Client
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public interface IApplicationClientConverting: IDisposable
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
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        Task SetFilesDataLoadedByClient(Guid filesDataId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task AbortConvertingById(Guid id);
    }
}