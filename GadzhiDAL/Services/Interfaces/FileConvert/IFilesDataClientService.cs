using System;
using System.Threading.Tasks;
using GadzhiDTOClient.TransferModels.FilesConvert;

namespace GadzhiDAL.Services.Interfaces.FileConvert
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах клиентской части
    /// </summary>
    public interface IFilesDataClientService
    {
        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary> 
        Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName);

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        Task<PackageDataShortResponseClient> GetFilesDataIntermediateResponseById(Guid id);

        /// <summary>
        /// Получить файл по номеру ID
        /// </summary>       
        Task<FileDataResponseClient> GetFileDataResponseById(Guid id, string filePath);

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        Task<PackageDataResponseClient> GetFilesDataResponseById(Guid id);

        /// <summary>
        /// Установить отметку о получении клиентом пакета. Переместить пакет в архив
        /// </summary>       
        Task SetFilesDataLoadedByClient(Guid id);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task AbortConvertingById(Guid id);
    }
}
