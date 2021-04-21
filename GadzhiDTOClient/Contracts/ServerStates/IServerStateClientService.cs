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
    }
}
