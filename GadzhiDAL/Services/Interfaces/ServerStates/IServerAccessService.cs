using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Interfaces.ServerStates
{
    /// <summary>
    /// Сервис определения времени доступа к серверам
    /// </summary>
    public interface IServerAccessService
    {
        /// <summary>
        /// Получить список серверов
        /// </summary>
        Task<IReadOnlyCollection<string>> GetServerNames();

        /// <summary>
        /// Получить последнее время доступа
        /// </summary>
        Task<DateTime?> GetLastAccess(string serverName);

        /// <summary>
        /// Записать время доступа
        /// </summary>
        Task UpdateLastAccess(string serverIdentity);
    }
}