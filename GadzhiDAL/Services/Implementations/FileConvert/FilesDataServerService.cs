﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommonServer.Enums;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.PaperSizes;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Archive;
using GadzhiDAL.Infrastructure.Implementations.Converters.Server;
using GadzhiDAL.Services.Interfaces.FileConvert;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.FileConvert
{
    /// <summary>
    /// Сервис для добавления и получения данных о конвертируемых пакетах серверной части
    /// </summary>
    public class FilesDataServerService : IFilesDataServerService
    {
        public FilesDataServerService(IUnityContainer container, IAccessService accessService)
        {
            _container = container;
            _accessService = accessService;
        }

        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Сервис определения времени доступа к серверам
        /// </summary>
        private readonly IAccessService _accessService;

        /// <summary>
        /// Получить первый в очереди пакет на конвертирование
        /// </summary>      
        public async Task<PackageDataRequestServer> GetFirstInQueuePackage(string identityServerName)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            await _accessService.UpdateLastServerAccess(identityServerName);
            var packageDataEntity = await unitOfWork.Session.Query<PackageDataEntity>().
                                          OrderBy(package => package.CreationDateTime).
                                          FirstOrDefaultAsync(ConditionConverting(identityServerName));
            var packageDataRequest = ExecuteResultHandler.ExecuteBindResultValue(
                () => ConverterFilesDataEntitiesToDtoServer.PackageDataToRequest(packageDataEntity));
            ValidatePackage(packageDataRequest, packageDataEntity, identityServerName);
            await unitOfWork.CommitAsync();
            return packageDataRequest.Value ?? PackageDataRequestServer.EmptyPackage;
        }

        /// <summary>
        /// Обновить информацию после промежуточного ответа. При отмене - удалить пакет
        /// </summary>      
        public async Task<StatusProcessingProject> UpdateFromIntermediateResponse(Guid packageId, FileDataResponseServer fileDataResponseServer)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var packageDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(packageId.ToString());

            if (!await DeleteFilesDataOnAbortionStatus(unitOfWork, packageDataEntity))
            {
                ConverterFilesDataEntitiesFromDtoServer.UpdateFileDataFromIntermediateResponse(packageDataEntity, fileDataResponseServer);
            }

            await PaperSizeUpdate(unitOfWork, fileDataResponseServer);
            await unitOfWork.CommitAsync();
            return packageDataEntity.StatusProcessingProject;
        }

        /// <summary>
        /// Обновить информацию после окончательного ответа. При отмене - удалить пакет
        /// </summary>      
        public async Task<Unit> UpdateFromResponse(PackageDataShortResponseServer packageDataShortResponseServer)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();
            var filesDataEntity = await unitOfWork.Session.LoadAsync<PackageDataEntity>(packageDataShortResponseServer.Id.ToString());

            if (!await DeleteFilesDataOnAbortionStatus(unitOfWork, filesDataEntity))
            {
                ConverterFilesDataEntitiesFromDtoServer.UpdatePackageDataFromShortResponse(filesDataEntity, packageDataShortResponseServer);
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
            var packageDataEntities = await unitOfWork.Session.Query<PackageDataEntity>().
                                      Where(filesData => filesData.CreationDateTime < dateDeletion).
                                      ToListAsync();

            foreach (var packageDataEntity in packageDataEntities)
            {
                ConverterToArchive.PackageDataToArchive(packageDataEntity);
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
        /// Добавить форматы
        /// </summary>
        private static async Task PaperSizeUpdate(IUnitOfWork unitOfWork, FileDataResponseServer fileDataResponseServer)
        {
            var paperSizes = fileDataResponseServer.FilesDataSource.
                             Select(fileDataSource => fileDataSource.PaperSize).Distinct().
                             Select(paperSize => new PaperSizeEntity(paperSize));
            foreach (var paperSize in paperSizes)
            {
                await unitOfWork.Session.SaveOrUpdateAsync(paperSize);
            }
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

        /// <summary>
        /// Проверка правильности загружаемого пакета
        /// </summary>
        private void ValidatePackage(IResultValue<PackageDataRequestServer> packageDataRequest,
                                     PackageDataEntity packageDataEntity, string identityServerName)
        {
            const int attemptingConvertCount = 2;
            if (packageDataRequest.OkStatus && packageDataEntity?.AttemptingConvertCount <= attemptingConvertCount)
            {
                packageDataEntity?.StartConverting(identityServerName);
            }
            else if (packageDataRequest.HasErrors && packageDataEntity?.AttemptingConvertCount <= attemptingConvertCount)
            {
                packageDataEntity?.AddAttemptingCount();
            }
            else
            {
                packageDataEntity?.ErrorConverting(identityServerName);
            }
        }
    }
}
