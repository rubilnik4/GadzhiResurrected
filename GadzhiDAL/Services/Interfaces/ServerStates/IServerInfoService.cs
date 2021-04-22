using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.ServerStates;

namespace GadzhiDAL.Services.Interfaces.ServerStates
{
    /// <summary>
    /// Информация о работе серверов в БД
    /// </summary>
    public interface IServerInfoService
    {
        /// <summary>
        /// Получить информацию серверах
        /// </summary>
        Task<ServersInfoResponse> GetServersInfo();

        /// <summary>
        /// Получить информацию о очереди на сервере
        /// </summary>
        Task<ServerDetailQueueResponse> GetServerDetailQueue(string serverName);
    }
}