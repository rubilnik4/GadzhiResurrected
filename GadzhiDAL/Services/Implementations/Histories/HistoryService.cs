using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
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
        /// Получить файла обработанных данных по идентификатору
        /// </summary>
        public async Task<IList<HistoryFileDataResponse>> GetHistoryFileData(Guid packageId)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.GetAsync<PackageDataEntity>(packageId.ToString());
            var packageDataArchiveEntity = await unitOfWork.Session.GetAsync<PackageDataArchiveEntity>(packageId.ToString());
            return (packageDataEntity, packageDataArchiveEntity) switch
            {
                (_, _) when packageDataEntity != null => 
                    GetHistoryFileBase<PackageDataEntity, FileDataEntity, FileDataSourceEntity>(packageDataEntity),
                (_, _) when packageDataArchiveEntity != null =>
                    GetHistoryFileBase<PackageDataArchiveEntity, FileDataArchiveEntity, FileDataSourceArchiveEntity>(packageDataArchiveEntity),
                (_, _) => new List<HistoryFileDataResponse>(),
            };
        }

        /// <summary>
        /// Получить список обработанных пакетов по дате
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
        /// Получить список обработанных пакетов по дате
        /// </summary>
        private static IList<HistoryFileDataResponse> GetHistoryFileBase<TEntity, TFileEntity, TFileSourceEntity>(TEntity package)
            where TEntity : PackageDataEntityBase<TFileEntity, TFileSourceEntity>
            where TFileEntity : FileDataEntityBase<TFileSourceEntity>
            where TFileSourceEntity : FileDataSourceEntityBase =>
            package.FileDataEntities.
            Select(fileDataEntity =>
                new HistoryFileDataResponse(fileDataEntity.FilePath, fileDataEntity.StatusProcessing,
                                            fileDataEntity.FileDataSourceServerEntities.
                                                           Select(fileSource => fileSource.FileExtensionType).
                                                           ToList(),
                                            fileDataEntity.FileErrors?.Count ?? 0)).
            ToList();


        /// <summary>
        /// Получить дату без времени
        /// </summary>
        private static DateTime GetDateTimeByDate(DateTime dateTime) =>
            new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}