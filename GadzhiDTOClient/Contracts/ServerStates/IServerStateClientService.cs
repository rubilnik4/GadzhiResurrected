using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.ServerStates;

namespace GadzhiDTOClient.Contracts.ServerStates
{
   /// <summary>
   /// Сервис получения информации о состоянии сервера
   /// </summary>
    [ServiceContract]
    public interface IServerStateClientService
    {
        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        [OperationContract]
        Task<ServerCompleteFilesResponse> GetServerCompleteFiles();

        /// <summary>
        /// Получить информацию серверах
        /// </summary>
        [OperationContract]
        Task<ServersInfoResponse> GetServersInfo();

        /// <summary>
        /// Получить список серверов
        /// </summary>
        [OperationContract]
        Task<IList<string>> GetServerNames();

        /// <summary>
        /// Получить информацию о сервере
        /// </summary>
        [OperationContract]
        Task<ServerDetailResponse> GetServerDetail(string serverName);
    }
}
