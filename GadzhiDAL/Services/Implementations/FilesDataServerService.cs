using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public class FilesDataServerService : IFilesDataServerService
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Конвертер из трансферной модели в модель базы данных
        /// </summary>
        private readonly IConverterDataAccessFilesDataFromDTOServer _converterDataAccessFilesDataFromDTOServer;

        /// <summary>
        /// Конвертер из модели базы данных в трансферную
        /// </summary>
        private readonly IConverterDataAccessFilesDataToDTOServer _converterDataAccessFilesDataToDTOServer;

        public FilesDataServerService(IUnityContainer container,
                                      IConverterDataAccessFilesDataFromDTOServer converterDataAccessFilesDataFromDTOServer,
                                      IConverterDataAccessFilesDataToDTOServer converterDataAccessFilesDataToDTOServer)
        {

            _container = container;
            _converterDataAccessFilesDataFromDTOServer = converterDataAccessFilesDataFromDTOServer;
            _converterDataAccessFilesDataToDTOServer = converterDataAccessFilesDataToDTOServer;
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>      
        public async Task<FilesDataRequestServer> GetFirstInQueuePackage(string identityServerName)
        {
            FilesDataRequestServer filesDataRequest = null;

            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  Query<FilesDataEntity>().
                                                  FirstOrDefaultAsync(ConditionConvertion(identityServerName));

                filesDataEntity?.StartConverting(identityServerName);
                filesDataRequest = await _converterDataAccessFilesDataToDTOServer.ConvertFilesDataAccessToRequest(filesDataEntity);

                await unitOfWork.CommitAsync();
            }

            return filesDataRequest;
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа. При отмене - удалить пакет
        /// </summary>      
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(FilesDataIntermediateResponseServer filesDataIntermediateResponse)
        {
            StatusProcessingProject statusProcessingProject = StatusProcessingProject.Converting;
            if (filesDataIntermediateResponse != null)
            {
                using (var unitOfWork = _container.Resolve<IUnitOfWork>())
                {
                    FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                      LoadAsync<FilesDataEntity>(filesDataIntermediateResponse.Id.ToString());

                    statusProcessingProject = filesDataEntity.StatusProcessingProject;
                    if (!await DeleteFilesDataOnAbortionStatus(unitOfWork, filesDataEntity))
                    {
                        filesDataEntity = _converterDataAccessFilesDataFromDTOServer.
                                           UpdateFilesDataAccessFromIntermediateResponse(filesDataEntity, filesDataIntermediateResponse);
                    }

                    await unitOfWork.CommitAsync();
                }
            }
            return statusProcessingProject;
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа. При отмене - удалить пакет
        /// </summary>      
        public async Task UpdateFromResponse(FilesDataResponseServer filesDataResponse)
        {
            if (filesDataResponse != null)
            {
                using (var unitOfWork = _container.Resolve<IUnitOfWork>())
                {
                    var filesDataEntity = await unitOfWork.Session.LoadAsync<FilesDataEntity>(filesDataResponse.Id.ToString());

                    if (!await DeleteFilesDataOnAbortionStatus(unitOfWork, filesDataEntity))
                    {
                        _converterDataAccessFilesDataFromDTOServer.UpdateFilesDataAccessFromResponse(filesDataEntity, filesDataResponse);
                    }

                    await unitOfWork.CommitAsync();
                }
            }
        }

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion)
        {
            using (var unitOfWork = _container.Resolve<IUnitOfWork>())
            {
                var filesDataEntity = await unitOfWork.Session.Query<FilesDataEntity>()?.
                                            Where(filesData => filesData.CreationDateTime < dateDeletion).ToListAsync();
                foreach (var fileData in filesDataEntity)
                {
                    await unitOfWork.Session.DeleteAsync(fileData);
                }
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
                FilesDataEntity filesDataEntity = await unitOfWork.Session.
                                                  LoadAsync<FilesDataEntity>(id.ToString());

                filesDataEntity?.AbortConverting(ClientServer.Server);

                await unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Проверить статус пакета. При отмене - удалить
        /// </summary>
        private async Task<bool> DeleteFilesDataOnAbortionStatus(IUnitOfWork unitOfWork, FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity.StatusProcessingProject == StatusProcessingProject.Abort)
            {
                await unitOfWork.Session.DeleteAsync(filesDataEntity);
            }
            return filesDataEntity.StatusProcessingProject == StatusProcessingProject.Abort;
        }

        /// <summary>
        /// Условие при котором пакет берется из очереди на конвертацию
        /// </summary> 
        private Expression<Func<FilesDataEntity, bool>> ConditionConvertion(string identityServerName)
        {
            return package => (package.StatusProcessingProject == StatusProcessingProject.InQueue ||
                               package.StatusProcessingProject == StatusProcessingProject.Converting &&
                               package.IdentityServerName == identityServerName);
        }
    }
}
