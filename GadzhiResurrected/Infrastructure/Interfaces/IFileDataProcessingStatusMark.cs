using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;

namespace GadzhiResurrected.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для получения файлов, у которых необходимо изменить статус
    /// </summary>
    public interface IFileDataProcessingStatusMark
    {
        /// <summary>
        /// Получить файлы готовые к отправке с байтовыми массивами
        /// </summary>       
        Task<IResultValue<PackageDataRequestClient>> GetFilesDataToRequest();

        /// <summary>
        /// Назначить всем файлам статус к отправке
        /// </summary>  
        PackageStatus GetFilesInSending();

        /// <summary>
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>  
        PackageStatus GetFilesNotFound(IEnumerable<FileDataRequestClient> fileDataRequest);

        /// <summary>5
        /// Пометить недоступные для отправки файлы ошибкой
        /// </summary>       
        PackageStatus GetPackageStatusIntermediateResponse(PackageDataShortResponseClient fileDataResponse);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        FileStatus GetFileStatusCompleteResponseBeforeWriting(FileDataResponseClient fileDataResponseClient);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета и перед записью файлов
        /// </summary>       
        PackageStatus GetFilesStatusCompleteResponseBeforeWriting(PackageDataResponseClient packageDataResponse);

        /// <summary>
        /// Поменять статус файла после записи
        /// </summary>       
        Task<FileStatus> GetFileStatusCompleteResponseAndWritten(FileDataResponseClient fileDataResponseClient);

        /// <summary>
        /// Поменять статус файлов после окончательного отчета
        /// </summary>       
        Task<PackageStatus> GetFilesStatusCompleteResponseAndWritten(PackageDataResponseClient packageDataResponse);

        /// <summary>
        /// Пометить неотправленные файлы ошибкой и изменить статус отправленных файлов
        /// </summary>
        PackageStatus GetPackageStatusAfterSend(PackageDataRequestClient packageDataRequest,
                                                PackageDataShortResponseClient packageDataShortResponse);
    }
}
