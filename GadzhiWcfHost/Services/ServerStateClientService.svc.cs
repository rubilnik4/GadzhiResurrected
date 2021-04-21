using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOBase.TransferModels.ServerStates;
using GadzhiDTOClient.Contracts.ServerStates;

namespace GadzhiWcfHost.Services
{
    /// <summary>
    /// Сервис получения информации о состоянии сервера
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true)]
    public class ServerStateClientService : IServerStateClientService
    {
        public ServerStateClientService(IServerStateService serverStateService)
        {
            _serverStateService = serverStateService;
        }

        /// <summary>
        /// Получение информации о серверах в БД
        /// </summary>
        private readonly IServerStateService _serverStateService;

        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        public async Task<ServerCompleteFilesResponse> GetServerCompleteFiles() =>
            await _serverStateService.GetServerCompleteFiles();
    }
}
