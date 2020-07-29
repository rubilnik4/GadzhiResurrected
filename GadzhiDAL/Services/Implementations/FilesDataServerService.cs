using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiDAL.Entities.FilesConvert.Errors;
using GadzhiDAL.Infrastructure.Implementations.Converters.Server;
using GadzhiDAL.Services.Interfaces;
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

        public FilesDataServerService(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>      
        public async Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.Query<PackageDataEntity>().
                                          OrderBy(package=> package.CreationDateTime).
                                          FirstOrDefaultAsync(ConditionConverting(identityServerName));

            packageDataEntity?.StartConverting(identityServerName);
            var packageDataRequest = ConverterFilesDataEntitiesToDtoServer.PackageDataToRequest(packageDataEntity);

            await unitOfWork.CommitAsync();

            return packageDataRequest ?? PackageDataRequestServer.EmptyPackage;
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа. При отмене - удалить пакет
        /// </summary>      
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(PackageDataIntermediateResponseServer packageDataIntermediateResponse)
        {
            if (packageDataIntermediateResponse == null) throw new ArgumentNullException(nameof(packageDataIntermediateResponse));

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(packageDataIntermediateResponse.Id.ToString());

            if (!await DeleteFilesDataOnAbortionStatus(unitOfWork, packageDataEntity))
            {
                packageDataEntity = ConverterFilesDataEntitiesFromDtoServer.UpdatePackageDataFromIntermediateResponse(packageDataEntity, packageDataIntermediateResponse);
            }

            await unitOfWork.CommitAsync();

            return packageDataEntity.StatusProcessingProject;
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа. При отмене - удалить пакет
        /// </summary>      
        public async Task<Unit> UpdateFromResponse(PackageDataResponseServer packageDataResponse)
        {
            if (packageDataResponse == null) throw new ArgumentNullException(nameof(packageDataResponse));

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var filesDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(packageDataResponse.Id.ToString());

            if (!await DeleteFilesDataOnAbortionStatus(unitOfWork, filesDataEntity))
            {
                ConverterFilesDataEntitiesFromDtoServer.UpdatePackageDataFromResponse(filesDataEntity, packageDataResponse);
            }

            await unitOfWork.CommitAsync();

            return Unit.Value;
        }

        /// <summary>
        /// Удалить все устаревшие пакеты
        /// </summary>      
        public async Task<Unit> DeleteAllUnusedPackagesUntilDate(DateTime dateDeletion)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var filesDataEntity = await unitOfWork.Session.Query<PackageDataEntity>().
                                        Where(filesData => filesData.CreationDateTime < dateDeletion).
                                        ToListAsync();
            foreach (var fileData in filesDataEntity)
            {
                await unitOfWork.Session.DeleteAsync(fileData);
            }
            await unitOfWork.CommitAsync();

            return Unit.Value;
        }

        /// <summary>
        /// Удалить все устаревшие пакеты с ошибками
        /// </summary>      
        public async Task<Unit> DeleteAllUnusedErrorPackagesUntilDate(DateTime dateDeletion)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var filesDataEntity = await unitOfWork.Session.Query<PackageDataErrorEntity>().
                                        Where(filesData => filesData.CreationDateTime < dateDeletion).
                                        ToListAsync();
            foreach (var fileData in filesDataEntity)
            {
                await unitOfWork.Session.DeleteAsync(fileData);
            }
            await unitOfWork.CommitAsync();

            return Unit.Value;
        }

        /// <summary>
        /// Отмена операции по номеру ID
        /// </summary>       
        public async Task<Unit> AbortConvertingById(Guid id)
        {
            if (id == Guid.Empty) return Unit.Value;

            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.GetAsync<PackageDataEntity>(id.ToString());

            packageDataEntity?.AbortConverting(ClientServer.Server);

            await unitOfWork.CommitAsync();

            return Unit.Value;
        }

        /// <summary>
        /// Проверить статус пакета. При отмене - удалить
        /// </summary>
        private static async Task<bool> DeleteFilesDataOnAbortionStatus(IUnitOfWork unitOfWork, PackageDataEntity packageDataEntity)
        {
            if (packageDataEntity.StatusProcessingProject == StatusProcessingProject.Abort)
            {
                await unitOfWork.Session.DeleteAsync(packageDataEntity);
            }
            return packageDataEntity.StatusProcessingProject == StatusProcessingProject.Abort;
        }

        /// <summary>
        /// Условие при котором пакет берется из очереди на конвертацию
        /// </summary> 
        private static Expression<Func<PackageDataEntity, bool>> ConditionConverting(string identityServerName) =>
            package => (package.StatusProcessingProject == StatusProcessingProject.InQueue ||
                        package.StatusProcessingProject == StatusProcessingProject.Converting &&
                        package.IdentityServerName == identityServerName);
    }
}
