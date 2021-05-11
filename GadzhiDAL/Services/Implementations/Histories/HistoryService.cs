using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Base;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Histories;
using GadzhiDAL.Services.Interfaces.Histories;
using GadzhiDTOBase.TransferModels.Histories;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.Histories
{
    /// <summary>
    /// Сервис получения истории конвертирования
    /// </summary>
    public class HistoryService : IHistoryService
    {
        public HistoryService(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Получить список пакетов
        /// </summary>
        public async Task<IList<HistoryDataResponse>> GetHistoryData(HistoryDataRequest historyDataRequest)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var historyArchive = await GetHistoryBase<PackageDataArchiveEntity, FileDataArchiveEntity, FileDataSourceArchiveEntity>(unitOfWork, historyDataRequest);
            var history = await GetHistoryBase<PackageDataEntity, FileDataEntity, FileDataSourceEntity>(unitOfWork, historyDataRequest);
            return historyArchive.Concat(history).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        private static async Task<IReadOnlyCollection<HistoryDataResponse>> GetHistoryBase<TEntity, TFileEntity, TFileSourceEntity>(IUnitOfWork unitOfWork,
                                                                                                    HistoryDataRequest historyDataRequest)
            where TEntity : PackageDataEntityBase<TFileEntity, TFileSourceEntity>
            where TFileEntity : FileDataEntityBase<TFileSourceEntity>
            where TFileSourceEntity : FileDataSourceEntityBase =>
            await unitOfWork.Session.Query<TEntity>().
            Where(package => package.CreationDateTime >= GetDateTimeByDate(historyDataRequest.DateTimeFrom) &&
                             package.CreationDateTime <= GetDateTimeByDate(historyDataRequest.DateTimeTo.AddDays(1)) &&
                             (String.IsNullOrWhiteSpace(historyDataRequest.ClientName) || package.IdentityLocalName == historyDataRequest.ClientName)).
            OrderBy(package => package.CreationDateTime).
            ToListAsync().
            MapAsync(HistoryDataConverter.ToResponses<TEntity, TFileEntity, TFileSourceEntity>);

        /// <summary>
        /// Получить дату без времени
        /// </summary>
        private static DateTime GetDateTimeByDate(DateTime dateTime) =>
            new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}