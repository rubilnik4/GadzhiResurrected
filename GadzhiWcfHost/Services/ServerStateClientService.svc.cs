using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Models.Enums.ServerStates;
using GadzhiDAL.Services.Interfaces;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDTOBase.TransferModels.ServerStates;
using GadzhiDTOClient.Contracts.ServerStates;
using GadzhiWcfHost.Infrastructure.Implementations.ServerStates;

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
        public ServerStateClientService(IServerStateService serverStateService, IServerInfoService serverInfoService,
                                        IAccessService accessService)
        {
            _serverStateService = serverStateService;
            _serverInfoService = serverInfoService;
            _accessService = accessService;
        }

        /// <summary>
        /// Получение информации о серверах в БД
        /// </summary>
        private readonly IServerStateService _serverStateService;

        /// <summary>
        /// Информация о работе серверов в БД
        /// </summary>
        private readonly IServerInfoService _serverInfoService;

        /// <summary>
        /// Сервис определения времени доступа к серверам
        /// </summary>
        private readonly IAccessService _accessService;

        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        public async Task<ServerCompleteFilesResponse> GetServerCompleteFiles() =>
            await _serverStateService.GetServerCompleteFiles();

        /// <summary>
        /// Получить информацию серверах
        /// </summary>
        public async Task<ServersInfoResponse> GetServersInfo() =>
            await _serverInfoService.GetServersInfo();

        /// <summary>
        /// Получить список серверов
        /// </summary>
        public async Task<IList<string>> GetServerNames() =>
            await _accessService.GetServerNames().
            MapAsync(serverNames => serverNames.ToList());

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        public async Task<IList<string>> GetClientNames() =>
            await _accessService.GetClientNames().
            MapAsync(clientNames => clientNames.ToList());

        /// <summary>
        /// Получить информацию о сервере
        /// </summary>
        public async Task<ServerDetailResponse> GetServerDetail(string serverName) =>
            await _accessService.GetServerLastAccess(serverName).
            WhereContinueAsyncBind(lastAccess => lastAccess.HasValue,
                okFunc: lastAccess => GetServerDetail(serverName, lastAccess!.Value),
                badFunc: _ => Task.FromResult(ServerDetailFactory.CreateNotInitialize(serverName)));

        /// <summary>
        /// Получить информацию о доступном сервере
        /// </summary>
        private async Task<ServerDetailResponse> GetServerDetail(string serverName, DateTime lastAccess) =>
            await _serverInfoService.GetServerDetailQueue(serverName).
            MapAsync(detailQueue =>
                new ServerDetailResponse(serverName, lastAccess, detailQueue,
                                         ServerDetailFactory.GetServerActivityType(lastAccess, detailQueue.CurrentPackage)));
        
    }
}
