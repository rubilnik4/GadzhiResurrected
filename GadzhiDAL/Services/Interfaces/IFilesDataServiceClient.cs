using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах клиентской части
    /// </summary>
    public interface IFilesDataServiceClient
    {
        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary> 
        Task QueueFilesData(FilesDataRequest filesDataRequest);

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        Task<FilesDataIntermediateResponse> GetFilesDataIntermediateResponseById(Guid id);

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        Task<FilesDataResponse> GetFilesDataResponseById(Guid id);
    }
}
