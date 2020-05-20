using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для получения файлов, у которых необходимо изменить статус
    /// </summary>
    public interface IFileDataProcessingStatusMark
    {
        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        Task<PackageDataRequestClient> GetFilesDataToRequest();

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        Task<PackageStatus> GetFilesInSending();

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        Task<PackageStatus> GetFilesNotFound(IEnumerable<FileDataRequestClient> fileDataRequest);

        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        Task<PackageStatus> GetPackageStatusIntermediateResponse(PackageDataIntermediateResponseClient fileDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        Task<PackageStatus> GetFilesStatusCompleteResponseBeforeWriting(PackageDataResponseClient packageDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета
        /// </summary>       
        Task<PackageStatus> GetFilesStatusCompleteResponseAndWritten(PackageDataResponseClient packageDataResponse);

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        Task<PackageStatus> GetPackageStatusAfterSend(PackageDataRequestClient packageDataRequest,
                                                                  PackageDataIntermediateResponseClient packageDataIntermediateResponse);
    }
}
