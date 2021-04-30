using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiDAL.Services.Interfaces.Histories;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiDTOClient.Contracts.Histories;

namespace GadzhiWcfHost.Services.Histories
{
    /// <summary>
    /// Сервис получения истории конвертирования
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class HistoryClientService : IHistoryClientService
    {
        public HistoryClientService(IAccessService accessService, IHistoryService historyService)
        {
            _accessService = accessService;
            _historyService = historyService;
        }

        /// <summary>
        /// Сервис определения времени доступа к серверам
        /// </summary>
        private readonly IAccessService _accessService;

        /// <summary>
        /// Сервис получения истории конвертирования
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        public async Task<IList<string>> GetClientNames() =>
            await _accessService.GetClientNames().
            MapAsync(clientNames => clientNames.ToList());

        /// <summary>
        /// Получить список пакетов
        /// </summary>
        public async Task<IList<HistoryDataResponse>> GetHistoryData(HistoryDataRequest historyDataRequest) =>
         await _historyService.GetHistoryData(historyDataRequest);
    }
}
