using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.ServerStates;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.DateTimes;
using GadzhiDAL.Services.Interfaces.ServerStates;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.ServerStates
{
    /// <summary>
    /// Сервис определения времени доступа
    /// </summary>
    public class AccessService: IAccessService
    {
        public AccessService(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Получить список серверов
        /// </summary>
        public async Task<IReadOnlyCollection<string>> GetServerNames() =>
            await GetIdentityNames<ServerAccessEntity>();

        /// <summary>
        /// Получить список клиентов
        /// </summary>
        public async Task<IReadOnlyCollection<string>> GetClientNames() =>
            await GetIdentityNames<ClientAccessEntity>();

        /// <summary>
        /// Получить список серверов
        /// </summary>
        private async Task<IReadOnlyCollection<string>> GetIdentityNames<TAccess>() 
            where TAccess: BaseAccessEntity
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var accessEntities = await unitOfWork.Session.Query<TAccess>().ToListAsync();
            return accessEntities.Select(entity => entity.Identity).ToList();
        }

        /// <summary>
        /// Получить последнее время доступа сервера
        /// </summary>
        public async Task<DateTime?> GetServerLastAccess(string identity) =>
            await GetLastAccess<ServerAccessEntity>(identity);

        /// <summary>
        /// Получить последнее время доступа клиента
        /// </summary>
        public async Task<DateTime?> GetClientLastAccess(string identity) =>
             await GetLastAccess<ClientAccessEntity>(identity);

        /// <summary>
        /// Получить последнее время доступа
        /// </summary>
        private async Task<DateTime?> GetLastAccess<TAccess>(string identity)
            where TAccess : BaseAccessEntity
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var serverAccessEntity =await unitOfWork.Session.LoadAsync<TAccess>(identity);
            return serverAccessEntity?.LastAccess;
        }

        /// <summary>
        /// Записать время доступа сервера
        /// </summary>
        public async Task UpdateLastServerAccess(string identity) =>
            await UpdateLastAccess(new ServerAccessEntity
            {
                Identity = identity,
                LastAccess = DateTimeService.GetDateTimeNow()
            });

        /// <summary>
        /// Записать время доступа сервера
        /// </summary>
        public async Task UpdateLastClientAccess(string identity) =>
            await UpdateLastAccess(new ClientAccessEntity
            {
                Identity = identity,
                LastAccess = DateTimeService.GetDateTimeNow()
            });

        /// <summary>
        /// Записать время доступа
        /// </summary>
        private async Task UpdateLastAccess<TAccess>(TAccess accessEntity) 
            where TAccess : BaseAccessEntity
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            await unitOfWork.Session.SaveOrUpdateAsync(accessEntity);
            await unitOfWork.CommitAsync();
        }
    }
}