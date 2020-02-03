using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using GadzhiWcfHost.Models.FilesConvert.Interfaces;
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
    public interface IApplicationConverting
    {
        /// <summary>
        /// Класс пользовательских пакетов на конвертирование
        /// </summary>
        IFilesDataPackages FilesDataPackages { get; }

        /// <summary>
        /// Поставить файлы в очередь для обработки
        /// </summary>
        void QueueFilesData(FilesDataServer filesDataServer);

        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        Task<FilesDataIntermediateResponse> QueueFilesDataAndGetResponse(FilesDataRequest filesDataRequest);

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        Task<FilesDataIntermediateResponse> GetIntermediateFilesDataResponseByID(Guid filesDataServerID);

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        Task<FilesDataResponse> GetFilesDataResponseByID(Guid filesDataServerID);
    }
}