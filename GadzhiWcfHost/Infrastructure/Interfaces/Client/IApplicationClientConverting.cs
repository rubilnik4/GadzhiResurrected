using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Enums.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiWcfHost.Infrastructure.Interfaces.Client
{
    /// <summary>
    /// Класс для сохранения, обработки, подготовки для отправки файлов
    /// </summary>
    public interface IApplicationClientConverting
    {
        /// <summary>
        /// Поместить файлы для конвертации в очередь и отправить ответ
        /// </summary>
        Task<PackageDataIntermediateResponseClient> QueueFilesDataAndGetResponse(PackageDataRequestClient packageDataRequest);

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов
        /// </summary>
        Task<PackageDataIntermediateResponseClient> GetIntermediateFilesDataResponseById(Guid packageId);

        /// <summary>
        /// Получить отконвертированные файлы
        /// </summary>
        Task<PackageDataResponseClient> GetFilesDataResponseById(Guid packageId);

        /// <summary>
        /// Установить отметку о получении клиентом пакета
        /// </summary>       
        Task SetFilesDataLoadedByClient(Guid packageId);

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        Task AbortConvertingById(Guid packageId);

        /// <summary>
        /// Загрузить имена из базы данных
        /// </summary>      
        Task<IList<SignatureDto>> GetSignaturesNames();

        /// <summary>
        /// Загрузить отделы из базы данных
        /// </summary>  
        Task<IList<DepartmentType>> GetSignaturesDepartments();
    }
}