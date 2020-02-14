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
using Unity;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public class FilesDataServiceServer : IFilesDataServiceServer
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

        public FilesDataServiceServer(IUnityContainer container,
                                      IConverterDataAccessFilesDataFromDTO converterDataAccessFilesDataFromDTO,
                                      IConverterDataAccessFilesDataToDTO converterDataAccessFilesDataToDTO)
        {

            _container = container;
            _converterDataAccessFilesDataFromDTO = converterDataAccessFilesDataFromDTO;
            _converterDataAccessFilesDataToDTO = converterDataAccessFilesDataToDTO;
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>      
        public async Task<FilesDataRequest> GetFirstInQueuePackage()
        {
            FilesDataRequest filesDataRequest = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  Query<FilesDataEntity>().
                                                  FirstOrDefaultAsync(package => !package.IsCompleted &&
                                                                      package.StatusProcessingProject == StatusProcessingProject.InQueue);
                if (filesDataEntity != null)
                {
                    filesDataEntity.StatusProcessingProject = StatusProcessingProject.Converting;
                    filesDataRequest = _converterDataAccessFilesDataToDTO.ConvertFilesDataAccessToRequest(filesDataEntity);
                }

                await unitOfWork.CommitAsync();
            }

            return filesDataRequest;
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа
        /// </summary>      
        public async Task UpdateFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            if (filesDataIntermediateResponse != null)
            {
                using (var unitOfWork = _container.Resolve<IUnitOfWork>())
                {
                    FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                      LoadAsync<FilesDataEntity>(filesDataIntermediateResponse.Id.ToString());

                    if (filesDataEntity != null)
                    {
                        filesDataEntity = _converterDataAccessFilesDataFromDTO.
                                           UpdateFilesDataAccessFromIntermediateResponse(filesDataEntity,
                                                                                         filesDataIntermediateResponse);
                    }

                    await unitOfWork.CommitAsync();
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
                using (var unitOfWork = _container.Resolve<IUnitOfWork>())
                {
                    FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                      LoadAsync<FilesDataEntity>(filesDataResponse.Id.ToString());

                    if (filesDataEntity != null)
                    {
                        filesDataEntity = _converterDataAccessFilesDataFromDTO.
                                           UpdateFilesDataAccessFromResponse(filesDataEntity,
                                                                             filesDataResponse);
                    }

                    await unitOfWork.CommitAsync();
                }
            }
        }
    }
}
