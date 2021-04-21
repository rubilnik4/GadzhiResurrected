using System.Threading.Tasks;
using GadzhiDTOBase.TransferModels.ServerStates;

namespace GadzhiDAL.Services.Interfaces
{
    /// <summary>
    /// Получение информации о серверах в БД
    /// </summary>
    public interface IServerStateService
    {
        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        Task<ServerCompleteFilesResponse> GetServerCompleteFiles();
    }
}