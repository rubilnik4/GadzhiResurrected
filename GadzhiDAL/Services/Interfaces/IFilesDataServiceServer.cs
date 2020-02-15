using GadzhiDTOServer.TransferModels.FilesConvert;
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
        Task<FilesDataRequestServer> GetFirstInQueuePackage(string identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        Task UpdateFromIntermediateResponse(FilesDataIntermediateResponseServer filesDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>      
        Task UpdateFromResponse(FilesDataResponseServer filesDataResponse);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task AbortConvertingById(Guid id);        
    }
}
