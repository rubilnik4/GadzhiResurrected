using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GadzhiDAL.Infrastructure.Implementations.Converters.Archive;
using GadzhiDAL.Infrastructure.Implementations.Converters.Client;
using GadzhiDAL.Services.Interfaces;
using Unity;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах клиентской части
    /// </summary>
    public class FilesDataClientService : IFilesDataClientService
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        public FilesDataClientService(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName)
        {
            var packageDataEntity = ConverterFilesDataEntitiesFromDtoClient.ToPackageData(packageDataRequest, identityName);

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            await unitOfWork.Session.SaveAsync(packageDataEntity);
            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        public async Task<PackageDataIntermediateResponseClient> GetFilesDataIntermediateResponseById(Guid id)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(id.ToString());
            var filesQueueInfo = await GetQueueCount(unitOfWork, packageDataEntity);

            return await ConverterFilesDataEntitiesToDtoClient.PackageDataToIntermediateResponse(packageDataEntity, filesQueueInfo);
        }

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        public async Task<PackageDataResponseClient> GetFilesDataResponseById(Guid id)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(id.ToString());
            var packageDataResponse = await ConverterFilesDataEntitiesToDtoClient.PackageDataAccessToResponse(packageDataEntity);

            await unitOfWork.CommitAsync();

            return packageDataResponse;
        }

        /// <summary>
        /// Установить отметку о получении клиентом пакета. Переместить пакет в архив
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid id)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(id.ToString());
            var packageDataArchiveEntity = await ConverterToArchive.PackageDataToArchive(packageDataEntity);

            await unitOfWork.Session.SaveAsync(packageDataArchiveEntity);
            await unitOfWork.Session.DeleteAsync(packageDataEntity);

            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            if (id == Guid.Empty) return;

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.GetAsync<PackageDataEntity>(id.ToString());

            packageDataEntity?.AbortConverting(ClientServer.Client);

            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получить данные о количестве пакетов и файлов в очереди на конвертирование
        /// </summary>
        private static async Task<FilesQueueInfo> GetQueueCount(IUnitOfWork unitOfWork, PackageDataEntity packageDataEntity)
        {
            var packageData = unitOfWork.Session.Query<PackageDataEntity>().
                              Where(package => !CheckStatusProcessing.CompletedStatusProcessingProject.Contains(package.StatusProcessingProject) &&
                                               package.CreationDateTime < packageDataEntity.CreationDateTime);

            int packagesInQueueCount = await packageData.CountAsync();
            int filesInQueueCount = await packageData.SelectMany(package => package.FileDataEntities).
                                                      Where(file => !CheckStatusProcessing.CompletedStatusProcessingServer.Contains(file.StatusProcessing)).
                                                      CountAsync();

            return new FilesQueueInfo(filesInQueueCount, packagesInQueueCount);
        }
    }
}
