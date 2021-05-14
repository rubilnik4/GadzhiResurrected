using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiDAL.Entities.FilesConvert;
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
            return await GetHistory(unitOfWork, historyDataRequest);
        }

        /// <summary>
        /// Получить файла обработанных данных по идентификатору
        /// </summary>
        public async Task<IList<HistoryFileDataResponse>> GetHistoryFileData(Guid packageId)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.GetAsync<PackageDataEntity>(packageId.ToString());
            return packageDataEntity switch
            {
                _ when packageDataEntity != null => GetHistoryFile(packageDataEntity),
                _ => new List<HistoryFileDataResponse>(),
            };
        }

        /// <summary>
        /// Получить список обработанных пакетов по дате
        /// </summary>
        private static async Task<IList<HistoryDataResponse>> GetHistory(IUnitOfWork unitOfWork,
                                                                                           HistoryDataRequest historyDataRequest) =>
            await unitOfWork.Session.Query<PackageDataEntity>().
            Where(package => package.CreationDateTime >= GetDateTimeByDate(historyDataRequest.DateTimeFrom) &&
                             package.CreationDateTime <= GetDateTimeByDate(historyDataRequest.DateTimeTo.AddDays(1)) &&
                             (String.IsNullOrWhiteSpace(historyDataRequest.ClientName) || package.IdentityLocalName == historyDataRequest.ClientName)).
            OrderBy(package => package.CreationDateTime).
            ToListAsync().
            MapAsync(HistoryDataConverter.ToResponses).
            MapAsync(historyData => historyData.ToList());

        /// <summary>
        /// Получить список обработанных пакетов по дате
        /// </summary>
        private static IList<HistoryFileDataResponse> GetHistoryFile(PackageDataEntity package) =>
            package.FileDataEntities.
            Select(fileDataEntity =>
                new HistoryFileDataResponse(fileDataEntity.FilePath, fileDataEntity.StatusProcessing,
                                            fileDataEntity.FileDataSourceServerEntities.
                                                           Select(fileSource => fileSource.FileExtensionType).
                                                           ToList(),
                                            fileDataEntity.FileErrors?.Count ?? 0, 
                                            fileDataEntity.FileDataSourceServerEntities.FirstOrDefault()?.
                                                           PaperSizes.ToList()
                                                           ?? new List<string>())).
            ToList();


        /// <summary>
        /// Получить дату без времени
        /// </summary>
        private static DateTime GetDateTimeByDate(DateTime dateTime) =>
            new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}