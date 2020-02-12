using GadzhiCommon.Enums.FilesConvert;
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
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public class FilesDataServiceServer : IFilesDataServiceServer
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

        public FilesDataServiceServer(IUnitOfWork unitOfWork,
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
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>      
        public async Task<FilesDataRequest> GetFirstInQueuePackage()
        {
            FilesDataEntity filesDataEntity;

            using (_unitOfWork.BeginTransaction())
            {
                filesDataEntity = await _repositoryFilesData.GetFirstOrDefaultAsync(package => !package.IsCompleted &&
                                                                                                package.StatusProcessingProject == StatusProcessingProject.InQueue);
                if (filesDataEntity != null)
                {
                    filesDataEntity.StatusProcessingProject = StatusProcessingProject.Converting;
                    await _repositoryFilesData.UpdateAsync(filesDataEntity);
                }

                await _unitOfWork.CommitAsync();
            }

            var filesDataRequest = _converterDataAccessFilesDataToDTO.ConvertFilesDataAccessToRequest(filesDataEntity);
            return filesDataRequest;
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        public async Task UpdateFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            if (filesDataIntermediateResponse != null)
            {
                using (_unitOfWork.BeginTransaction())
                {
                    FilesDataEntity filesDataEntity =
                        await _repositoryFilesData.LoadAsync(filesDataIntermediateResponse.Id.ToString());

                    if (filesDataEntity != null)
                    {
                        filesDataEntity = _converterDataAccessFilesDataFromDTO.
                                           UpdateFilesDataAccessFromIntermediateResponse(filesDataEntity,
                                                                                         filesDataIntermediateResponse);               
                    }

                    await _unitOfWork.CommitAsync();
                }
            }
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа
        /// </summary>      
        public async Task UpdateFromResponse(FilesDataResponse filesDataResponse)
        {
            if (filesDataResponse != null)
            {
                using (_unitOfWork.BeginTransaction())
                {
                    FilesDataEntity filesDataEntity =
                        await _repositoryFilesData.LoadAsync(filesDataResponse.Id.ToString());

                    if (filesDataEntity != null)
                    {
                        filesDataEntity = _converterDataAccessFilesDataFromDTO.
                                           UpdateFilesDataAccessFromResponse(filesDataEntity,
                                                                             filesDataResponse);                       
                    }

                    await _unitOfWork.CommitAsync();
                }
            }
        }
    }
}
