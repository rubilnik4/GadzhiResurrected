using System;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Archive;
using GadzhiDAL.Infrastructure.Implementations.Converters.Client;
using GadzhiDAL.Infrastructure.Implementations.Converters.Errors;
using GadzhiDAL.Models.Implementations;
using GadzhiDAL.Services.Interfaces.FileConvert;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.FileConvert
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

        public FilesDataClientService(IUnityContainer container, IAccessService accessService)
        {
            _container = container;
            _accessService = accessService;
        }

        /// <summary>
        /// Сервис определения времени доступа
        /// </summary>
        private readonly IAccessService _accessService;

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task QueueFilesData(PackageDataRequestClient packageDataRequest, string identityName)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            await _accessService.UpdateLastClientAccess(identityName);
            var packageDataEntity = ConverterFilesDataEntitiesFromDtoClient.ToPackageData(packageDataRequest, identityName);
            await unitOfWork.Session.SaveAsync(packageDataEntity);
            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        public async Task<PackageDataShortResponseClient> GetFilesDataIntermediateResponseById(Guid id)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(id.ToString());
            var filesQueueInfo = await GetQueueCount(unitOfWork, packageDataEntity);

            return ConverterFilesDataEntitiesToDtoClient.PackageDataToIntermediateResponse(packageDataEntity, filesQueueInfo);
        }

        /// <summary>
        /// Получить файл по номеру ID
        /// </summary>       
        public async Task<FileDataResponseClient> GetFileDataResponseById(Guid id, string filePath)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(id.ToString());
            var fileDataEntity = packageDataEntity.FileDataEntities.First(entity => entity.FilePath == filePath);
            var packageDataResponse = await ConverterFilesDataEntitiesToDtoClient.FileDataAccessToResponse(fileDataEntity);

            return packageDataResponse;
        }

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        public async Task<PackageDataResponseClient> GetFilesDataResponseById(Guid id)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(id.ToString());
            var packageDataResponse = await ConverterFilesDataEntitiesToDtoClient.PackageDataAccessToResponse(packageDataEntity);

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

            await SetFileDataToErrorStore(unitOfWork, packageDataEntity);
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
                                                      Where(file => !CheckStatusProcessing.CompletedStatusProcessing.Contains(file.StatusProcessing)).
                                                      CountAsync();

            return new FilesQueueInfo(filesInQueueCount, packagesInQueueCount);
        }

        /// <summary>
        /// Переместить файлы в хранилище ошибок при наличии
        /// </summary>
        private static async Task SetFileDataToErrorStore(IUnitOfWork unitOfWork, PackageDataEntity packageDataEntity)
        {
            if (packageDataEntity.FileDataEntities.Any(fileData => fileData.FileErrors.Count > 0))
            {
                var packageDataErrorEntity = await ConverterToErrorsStore.PackageDataToErrorStore(packageDataEntity);
                await unitOfWork.Session.SaveAsync(packageDataErrorEntity);
            }
        }
    }
}
