using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах
    /// </summary>
    public interface IFilesDataService
    {
        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary> 
        Task QueueFilesData(FilesDataRequest filesDataRequest);

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        Task<FilesDataIntermediateResponse> GetIntermediateFilesDataById(Guid id);
    }
}
