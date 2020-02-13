using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDTO.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах клиентской части
    /// </summary>
    public class FilesDataServiceClient : IFilesDataServiceClient
    {
        /// <summary>
        /// Класс обертка для управления транзакциями
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Репозиторий для конвертируемых файлов
        /// </summary>
        private readonly IRepository<FilesDataEntity, string> _repositoryFilesData;

        /// <summary>
        /// Конвертер из трансферной модели в модель базы данных
        /// </summary>
        private readonly IConverterDataAccessFilesDataFromDTO _converterDataAccessFilesDataFromDTO;

        /// <summary>
        /// Конвертер из модели базы данных в трансферную
        /// </summary>
        private readonly IConverterDataAccessFilesDataToDTO _converterDataAccessFilesDataToDTO;

        public FilesDataServiceClient(IUnitOfWork unitOfWork,
                                IRepository<FilesDataEntity, string> repositoryFilesData,
                                IConverterDataAccessFilesDataFromDTO converterDataAccessFilesDataFromDTO,
                                IConverterDataAccessFilesDataToDTO converterDataAccessFilesDataToDTO)
        {
            _unitOfWork = unitOfWork;
            _repositoryFilesData = repositoryFilesData;
            _converterDataAccessFilesDataFromDTO = converterDataAccessFilesDataFromDTO;
            _converterDataAccessFilesDataToDTO = converterDataAccessFilesDataToDTO;
        }

        /// <summary>
        /// Добавить пакет в очередь на конвертирование в базу
        /// </summary>       
        public async Task QueueFilesData(FilesDataRequest filesDataRequest)
        {
            FilesDataEntity filesDataEntity = _converterDataAccessFilesDataFromDTO.ConvertToFilesDataAccess(filesDataRequest);

            using (_unitOfWork.BeginTransaction())
            {
                await _repositoryFilesData.AddAsync(filesDataEntity);
                await _unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Получить промежуточный ответ о состоянии конвертируемых файлов по номеру ID
        /// </summary>       
        public async Task<FilesDataIntermediateResponse> GetIntermediateFilesDataById(Guid id)
        {
            FilesDataEntity filesDataEntity;
            int filesInQueueCount = 0;
            int packagesInQueueCount = 0;          

            using (_unitOfWork.BeginTransaction())
            {
                filesDataEntity = await _repositoryFilesData.LoadAsync(id.ToString());
                (filesInQueueCount, packagesInQueueCount) = await GetQueueCount(filesDataEntity);
            }

            FilesDataIntermediateResponse filesDataIntermediateResponse = _converterDataAccessFilesDataToDTO.
                                                                          ConvertFilesDataAccessToIntermediateResponse(filesDataEntity);
            filesDataIntermediateResponse.FilesQueueInfo = new FilesQueueInfoResponse()
            {
                FilesInQueueCount = filesInQueueCount,
                PackagesInQueueCount = packagesInQueueCount,
            };

            return filesDataIntermediateResponse;
        }       

        /// <summary>
        /// Получить данные о количестве пакетов и файлов в очереди на конвертирование
        /// </summary>
        private async Task<(int filesInQueueCount, int packagesInQueueCount)> GetQueueCount(FilesDataEntity filesDataEntity)
        {           
            var filesDataTask = _repositoryFilesData.Query().
                                                     Where(package => filesDataEntity != null &&
                                                                      !package.IsCompleted &&
                                                                      package.CreationDateTime < filesDataEntity.CreationDateTime);
            int packagesInQueueCount = await filesDataTask?.CountAsync();
            int filesInQueueCount = await filesDataTask?.SelectMany(package => package.FilesData).
                                                     Where(file => !file.IsCompleted).
                                                     CountAsync();

            return (filesInQueueCount, packagesInQueueCount);

        }
    }
}
