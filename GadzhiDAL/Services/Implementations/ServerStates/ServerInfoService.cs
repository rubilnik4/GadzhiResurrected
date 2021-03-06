﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDTOBase.TransferModels.ServerStates;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.ServerStates
{
    /// <summary>
    /// Информация о работе серверов в БД
    /// </summary>
    public class ServerInfoService : IServerInfoService
    {
        public ServerInfoService(IUnityContainer container, IAccessService accessService)
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
        /// Получить информацию серверах
        /// </summary>
        public async Task<ServersInfoResponse> GetServersInfo()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var serverNames = await _accessService.GetServerNames();
            int completePackages = await GetCompletePackages(unitOfWork);
            int completeFiles = await GetCompleteFiles(unitOfWork);
            int queuePackages = await GetQueuePackages(unitOfWork);
            int queueFiles = await GetQueueFiles(unitOfWork);
            return new ServersInfoResponse(serverNames, completePackages, completeFiles, queuePackages, queueFiles);
        }

        /// <summary>
        /// Получить информацию о очереди на сервере
        /// </summary>
        public async Task<ServerDetailQueueResponse> GetServerDetailQueue(string serverName)
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            (string currentUser, string currentPackage, string currentFile, int filesInQueue) = await GetCurrentPackage(unitOfWork, serverName);
            (int packages, int files) = await GetCompletePackagesAndFiles(unitOfWork, serverName);
            return new ServerDetailQueueResponse(currentUser, currentPackage, currentFile, filesInQueue,
                                                 packages, files);
        }

        /// <summary>
        /// Получить количество готовых пакетов
        /// </summary>
        private static async Task<int> GetCompletePackages(IUnitOfWork unitOfWork) =>
            await unitOfWork.Session.
            Query<PackageDataEntity>().
            CountAsync(package => CheckStatusProcessing.CompletedStatusProcessingProject.Contains(package.StatusProcessingProject));

        /// <summary>
        /// Получить количество готовых пакетов по серверу
        /// </summary>
        private static async Task<(int, int)> GetCompletePackagesAndFiles(IUnitOfWork unitOfWork, string serverName) =>
            await unitOfWork.Session.Query<PackageDataEntity>().
            Where(package => package.IdentityServerName == serverName &&
                             CheckStatusProcessing.OperatingStatusProcessingProject.Contains(package.StatusProcessingProject)).
            Select(package => new { Status = package.StatusProcessingProject,
                                    FilesCount = package.FileDataEntities.
                                                 Count(fileData => CheckStatusProcessing.CompletedStatusProcessing.Contains(fileData.StatusProcessing)) }).
            ToListAsync().
            MapAsync(packagesCount => (packagesCount.Count(packageCount => CheckStatusProcessing.CompletedStatusProcessingProject.Contains(packageCount.Status)),
                                       packagesCount.Sum(packageCount => packageCount.FilesCount)));

        /// <summary>
        /// Получить количество готовых файлов
        /// </summary>
        private static async Task<int> GetCompleteFiles(IUnitOfWork unitOfWork) =>
            await unitOfWork.Session.
            Query<FileDataEntity>().
            CountAsync(fileData => CheckStatusProcessing.CompletedStatusProcessing.Contains(fileData.StatusProcessing));

        /// <summary>
        /// Получить количество пакетов в очереди
        /// </summary>
        private static async Task<int> GetQueuePackages(IUnitOfWork unitOfWork) =>
            await unitOfWork.Session.
            Query<PackageDataEntity>().
            CountAsync(package => !CheckStatusProcessing.CompletedStatusProcessingProject.Contains(package.StatusProcessingProject));

        /// <summary>
        /// Получить количество файлов в очереди
        /// </summary>
        private static async Task<int> GetQueueFiles(IUnitOfWork unitOfWork) =>
            await unitOfWork.Session.
            Query<FileDataEntity>().
            CountAsync(package => !CheckStatusProcessing.CompletedStatusProcessing.Contains(package.StatusProcessing));

        /// <summary>
        /// Получить текущие файлы конвертирования
        /// </summary>
        public async Task<(string, string, string, int)> GetCurrentPackage(IUnitOfWork unitOfWork, string serverName) =>
            await unitOfWork.Session.
            Query<PackageDataEntity>().
            FirstOrDefaultAsync(package => package.IdentityServerName == serverName &&
                                           package.StatusProcessingProject == StatusProcessingProject.Converting).
            MapAsync(package => (package?.IdentityLocalName ?? String.Empty,
                                 package?.Id ?? String.Empty,
                                 package?.FileDataEntities.
                                          FirstOrDefault(fileData => !CheckStatusProcessing.CompletedStatusProcessing.Contains(fileData.StatusProcessing))?.
                                          FilePath 
                                 ?? String.Empty,
                                 package?.FileDataEntities.
                                          Count(fileData => !CheckStatusProcessing.CompletedStatusProcessing.Contains(fileData.StatusProcessing)) 
                                 ?? 0));
    }
}