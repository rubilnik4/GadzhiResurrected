using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDTO.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public interface IFilesDataServiceServer
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование в серверной части
        /// </summary>      
        Task<FilesDataRequest> GetFirstInQueuePackage();

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        Task UpdateFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>      
        Task UpdateFromResponse(FilesDataResponse filesDataResponse);
    }
}
