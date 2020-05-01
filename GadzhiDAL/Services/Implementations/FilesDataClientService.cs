using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Archive;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        /// <summary>
        /// Конвертер из трансферной модели в модель базы данных
        /// </summary>
        private readonly IConverterDataAccessFilesDataFromDTOClient _converterDataAccessFilesDataFromDTOClient;

        /// <summary>
        /// Конвертер из модели базы данных в трансферную
        /// </summary>
        private readonly IConverterDataAccessFilesDataToDTOClient _converterDataAccessFilesDataToDTOClient;

        /// <summary>
        /// Конвертер в архивную версию
        /// </summary>    
        private readonly IConverterToArchive _сonverterToArchive;

        public FilesDataClientService(IUnityContainer container,
                                      IConverterDataAccessFilesDataFromDTOClient converterDataAccessFilesDataFromDTOClient,
                                      IConverterDataAccessFilesDataToDTOClient converterDataAccessFilesDataToDTOClient,
                                      IConverterToArchive сonverterToArchive)
        {
            _container = container;
            _converterDataAccessFilesDataFromDTOClient = converterDataAccessFilesDataFromDTOClient;
            _converterDataAccessFilesDataToDTOClient = converterDataAccessFilesDataToDTOClient;
            _сonverterToArchive = сonverterToArchive;
        }

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task QueueFilesData(PackageDataRequestClient packageDataRequest)
        {
            FilesDataEntity filesDataEntity = _converterDataAccessFilesDataFromDTOClient.ConvertToFilesDataAccess(packageDataRequest);

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                await unitOfWork.Session.SaveAsync(filesDataEntity);
                await unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        public async Task<PackageDataIntermediateResponseClient> GetFilesDataIntermediateResponseById(Guid id)
        {
            PackageDataIntermediateResponseClient packageDataIntermediateResponse = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                FilesQueueInfo filesQueueInfo = await GetQueueCount(unitOfWork, filesDataEntity);

                packageDataIntermediateResponse = await _converterDataAccessFilesDataToDTOClient.
                                                       ConvertFilesDataAccessToIntermediateResponse(filesDataEntity, filesQueueInfo);
            }

            return packageDataIntermediateResponse;
        }

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        public async Task<PackageDataResponseClient> GetFilesDataResponseById(Guid id)
        {
            PackageDataResponseClient packageDataResponse = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.LoadAsync<FilesDataEntity>(id.ToString());
                packageDataResponse = await _converterDataAccessFilesDataToDTOClient.ConvertFilesDataAccessToResponse(filesDataEntity);

                await unitOfWork.CommitAsync();
            }

            return packageDataResponse;
        }

        /// <summary>
        /// Установить отметку о получении клиентом пакета. Переместить пакет в архив
        /// </summary>       
        public async Task SetFilesDataLoadedByClient(Guid id)
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.LoadAsync<FilesDataEntity>(id.ToString());

                FilesDataArchiveEntity filesDataArchiveEntity = await _сonverterToArchive.ConvertFilesDataToArchive(filesDataEntity);
                await unitOfWork.Session.SaveAsync(filesDataArchiveEntity);
                await unitOfWork.Session.DeleteAsync(filesDataEntity);

                await unitOfWork.CommitAsync();
            }
        }
        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.GetAsync<FilesDataEntity>(id.ToString());

                filesDataEntity?.AbortConverting(ClientServer.Client);

                await unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Получить данные о количестве пакетов и файлов в очереди на конвертирование
        /// </summary>
        private async Task<FilesQueueInfo> GetQueueCount(IUnitOfWork unitOfWork, FilesDataEntity filesDataEntity)
        {
            var filesDataTask = unitOfWork.Session.Query<FilesDataEntity>().
                                                    Where(package => filesDataEntity != null &&
                                                                     !CheckStatusProcessing.CompletedStatusProcessingProject.Contains(package.StatusProcessingProject) &&
                                                                     package.CreationDateTime < filesDataEntity.CreationDateTime);

            int packagesInQueueCount = await filesDataTask?.CountAsync();
            int filesInQueueCount = await filesDataTask?.SelectMany(package => package.FileDataEntities).
                                                         Where(file => !CheckStatusProcessing.CompletedStatusProcessingServer.Contains(file.StatusProcessing)).
                                                         CountAsync();

            return new FilesQueueInfo(filesInQueueCount, packagesInQueueCount);
        }
    }
}
