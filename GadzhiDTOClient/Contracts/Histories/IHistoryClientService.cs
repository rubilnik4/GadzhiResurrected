using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.Histories;

namespace GadzhiDTOClient.Contracts.Histories
{
    /// <summary>
    /// Сервис получения истории конвертирования
    /// </summary>
    [ServiceContract]
    public interface IHistoryClientService
    {
        /// <summary>
        /// Получить список пользователей
        /// </summary>
        [OperationContract]
        Task<IList<string>> GetClientNames();

        /// <summary>
        /// Получить список пакетов
        /// </summary>
        [OperationContract]
        Task<IList<HistoryDataResponse>> GetHistoryData(HistoryDataRequest historyDataRequest);
    }
}
