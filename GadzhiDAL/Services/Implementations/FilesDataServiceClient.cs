using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDAL.Models.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
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
    public class FilesDataServiceClient : IFilesDataServiceClient
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Конвертер из трансферной модели в модель базы данных
        /// </summary>
        private readonly IConverterDataAccessFilesDataFromDTO _converterDataAccessFilesDataFromDTO;

        /// <summary>
        /// Конвертер из модели базы данных в трансферную
        /// </summary>
        private readonly IConverterDataAccessFilesDataToDTO _converterDataAccessFilesDataToDTO;

        public FilesDataServiceClient(IUnityContainer container,
                                      IConverterDataAccessFilesDataFromDTO converterDataAccessFilesDataFromDTO,
                                      IConverterDataAccessFilesDataToDTO converterDataAccessFilesDataToDTO)
        {
            _container = container;
            _converterDataAccessFilesDataFromDTO = converterDataAccessFilesDataFromDTO;
            _converterDataAccessFilesDataToDTO = converterDataAccessFilesDataToDTO;
        }

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task QueueFilesData(FilesDataRequest filesDataRequest)
        {
            FilesDataEntity filesDataEntity = _converterDataAccessFilesDataFromDTO.ConvertToFilesDataAccess(filesDataRequest);

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                await unitOfWork.Session.SaveAsync(filesDataEntity);
                await unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        public async Task<FilesDataIntermediateResponse> GetFilesDataIntermediateResponseById(Guid id)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                FilesQueueInfo filesQueueInfo = await GetQueueCount(unitOfWork, filesDataEntity);

                filesDataIntermediateResponse = _converterDataAccessFilesDataToDTO.
                                                ConvertFilesDataAccessToIntermediateResponse(filesDataEntity,
                                                                                             filesQueueInfo);
            }

            return filesDataIntermediateResponse;
        }

        /// <summary>
        /// Получить окончательный пакет отконвертированных файлов по номеру ID
        /// </summary>       
        public async Task<FilesDataResponse> GetFilesDataResponseById(Guid id)
        {
            FilesDataResponse filesDataResponse = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                filesDataResponse = _converterDataAccessFilesDataToDTO.
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
