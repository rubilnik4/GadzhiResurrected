using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.FilesConvert.ServerStates;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.DateTimes;
using GadzhiDAL.Services.Interfaces.ServerStates;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.ServerStates
{
    /// <summary>
    /// Сервис определения времени доступа к серверам
    /// </summary>
    public class ServerAccessService: IServerAccessService
    {
        public ServerAccessService(IUnityContainer container)
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
        public async Task<IReadOnlyCollection<string>> GetServerNames()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var serverAccessEntities = await unitOfWork.Session.Query<ServerAccessEntity>().ToListAsync();
            return serverAccessEntities.Select(serverAccess => serverAccess.ServerIdentity).ToList();
        }

        /// <summary>
        /// Получить последнее время доступа
        /// </summary>
        public async Task<DateTime?> GetLastAccess(string serverName)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var serverAccessEntity =await unitOfWork.Session.LoadAsync<ServerAccessEntity>(serverName);
            return serverAccessEntity?.LastAccess;
        }

        /// <summary>
        /// Записать время доступа
        /// </summary>
        public async Task UpdateLastAccess(string serverIdentity)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var serverAccessEntity = new ServerAccessEntity
            {
                ServerIdentity = serverIdentity,
                LastAccess = DateTimeService.GetDateTimeNow(),
            };
            await unitOfWork.Session.SaveOrUpdateAsync(serverAccessEntity);
            await unitOfWork.CommitAsync();
        }
    }
}