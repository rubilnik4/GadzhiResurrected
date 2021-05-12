using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.Histories;

namespace GadzhiDAL.Services.Interfaces.Histories
{
    /// <summary>
    /// Сервис получения истории конвертирования
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// Получить список пакетов
        /// </summary>
        Task<IList<HistoryDataResponse>> GetHistoryData(HistoryDataRequest historyDataRequest);

        /// <summary>
        /// Получить файла обработанных данных по идентификатору
        /// </summary>
        Task<IList<HistoryFileDataResponse>> GetHistoryFileData(Guid packageId);
    }
}