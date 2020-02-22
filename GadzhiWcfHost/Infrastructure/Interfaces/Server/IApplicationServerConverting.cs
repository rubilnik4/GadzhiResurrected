using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Interfaces.Server
{
    /// <summary>
    /// Класс для отправки пакетов на сервер
    /// </summary>
    public interface IApplicationServerConverting: IDisposable
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
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