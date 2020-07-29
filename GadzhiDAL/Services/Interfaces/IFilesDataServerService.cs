using System;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDTOServer.TransferModels.FilesConvert;

namespace GadzhiDAL.Services.Interfaces
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public interface IFilesDataServerService
    {
        /// <summary>
        /// Получить первый в очереди пакет на конвертирование в серверной части
        /// </summary>      
        Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName);

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse);

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>      
        Task<Unit> UpdateFromResponse(PackageDataResponseServer packageDataResponse);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task<Unit> AbortConvertingById(Guid id);

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        Task<Unit> DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion);

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        Task<Unit> DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion);
    }
}
