using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public FilesDataClientService(IUnityContainer container,
                                      IConverterDataAccessFilesDataFromDTOClient converterDataAccessFilesDataFromDTOClient,
                                      IConverterDataAccessFilesDataToDTOClient converterDataAccessFilesDataToDTOClient)
        {
            _container = container;
            _converterDataAccessFilesDataFromDTOClient = converterDataAccessFilesDataFromDTOClient;
            _converterDataAccessFilesDataToDTOClient = converterDataAccessFilesDataToDTOClient;
        }

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task QueueFilesData(FilesDataRequestClient filesDataRequest)
        {
            FilesDataEntity filesDataEntity = _converterDataAccessFilesDataFromDTOClient.ConvertToFilesDataAccess(filesDataRequest);

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                await unitOfWork.Session.SaveAsync(filesDataEntity);
                await unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        public async Task<FilesDataIntermediateResponseClient> GetFilesDataIntermediateResponseById(Guid id)
        {
            FilesDataIntermediateResponseClient filesDataIntermediateResponse = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                FilesQueueInfo filesQueueInfo = await GetQueueCount(unitOfWork, filesDataEntity);

                filesDataIntermediateResponse = _converterDataAccessFilesDataToDTOClient.
                                                ConvertFilesDataAccessToIntermediateResponse(filesDataEntity,
                                                                                             filesQueueInfo);
            }

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        public async Task<FilesDataResponseClient> GetFilesDataResponseById(Guid id)
        {
            FilesDataResponseClient filesDataResponse = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                filesDataResponse = _converterDataAccessFilesDataToDTOClient.
                                        ConvertFilesDataAccessToResponse(filesDataEntity);              
            }

            return filesDataResponse;
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task AbortConvertingById(Guid id)
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                filesDataEntity?.AbortConverting(ClientServer.Client);

                await unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Получить данные о количестве пакетов и файлов в очереди на конвертирование
        /// </summary>
        private async Task<FilesQueueInfo> GetQueueCount(IUnitOfWork unityOfWork,
                                                         FilesDataEntity filesDataEntity)
        {
            var filesDataTask = unityOfWork.Session.Query<FilesDataEntity>().
                                                    Where(package => filesDataEntity != null &&
                                                                     !package.IsCompleted &&                                                                   
                                                                     package.CreationDateTime < filesDataEntity.CreationDateTime);

            int packagesInQueueCount = await filesDataTask?.CountAsync();
            int filesInQueueCount = await filesDataTask?.SelectMany(package => package.FilesData).
                                                         Where(file => !file.IsCompleted).
                                                         CountAsync();

            return new FilesQueueInfo(filesInQueueCount,
                                      packagesInQueueCount);

        }
    }
}
