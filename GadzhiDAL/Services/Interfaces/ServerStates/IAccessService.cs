using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Interfaces.ServerStates
{
    /// <summary>
    /// Сервис определения времени доступа
    /// </summary>
    public interface IAccessService
    {
        /// <summary>
        /// Получить список серверов
        /// </summary>
        Task<IReadOnlyCollection<string>> GetServerNames();

        /// <summary>
        /// Получить список клиентов
        /// </summary>
        Task<IReadOnlyCollection<string>> GetClientNames();

        /// <summary>
        /// Получить последнее время доступа сервера
        /// </summary>
        Task<DateTime?> GetServerLastAccess(string identity);

        /// <summary>
        /// Получить последнее время доступа клиента
        /// </summary>
        Task<DateTime?> GetClientLastAccess(string identity);

        /// <summary>
        /// Записать время доступа сервера
        /// </summary>
        Task UpdateLastServerAccess(string identity);

        /// <summary>
        /// Записать время доступа сервера
        /// </summary>
        Task UpdateLastClientAccess(string identity);
    }
}