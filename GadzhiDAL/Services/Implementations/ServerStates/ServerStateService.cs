using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Base;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Models.Implementations;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDTOBase.TransferModels.ServerStates;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations.ServerStates
{
    /// <summary>
    /// Получение информации о серверах в БД
    /// </summary>
    public class ServerStateService : IServerStateService
    {
        public ServerStateService(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        public async Task<ServerCompleteFilesResponse> GetServerCompleteFiles()
        {
            using var unitOfWork = _container.Resolve<IUnitOfWork>();

            var countSourceArchive = await GetServerSourceCompleteFiles<FileDataSourceArchiveEntity>(unitOfWork);
            var countSource = await GetServerSourceCompleteFiles<FileDataSourceEntity>(unitOfWork);

            return GetServerCompleteFiles(countSourceArchive, countSource);
        }

        /// <summary>
        /// Получить информацию о обработанных файлах из архива и текущего списка
        /// </summary>
        private static async Task<IReadOnlyCollection<FileExtensionCount>> GetServerSourceCompleteFiles <TSource>(IUnitOfWork unitOfWork)
            where TSource: FileDataSourceEntityBase =>
            await unitOfWork.Session.
            Query<TSource>().
            GroupBy(entity => entity.FileExtensionType).
            Select(group => new { Key = group.Key, Count = group.Count() }).
            ToListAsync().
            MapAsync(completeFiles => completeFiles.Select(completeFile => new FileExtensionCount(completeFile.Key, completeFile.Count)).ToList());

        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        private static ServerCompleteFilesResponse GetServerCompleteFiles(IEnumerable<FileExtensionCount> sourceArchiveCount,
                                                                          IEnumerable<FileExtensionCount> sourceCount) =>
            sourceArchiveCount.
            Union(sourceCount).
            ToList().
            Map(completeFiles => new ServerCompleteFilesResponse(GetCompleteFilesCount(completeFiles, FileExtensionType.Dgn),
                                                                 GetCompleteFilesCount(completeFiles, FileExtensionType.Doc) +
                                                                 GetCompleteFilesCount(completeFiles, FileExtensionType.Docx),
                                                                 GetCompleteFilesCount(completeFiles, FileExtensionType.Pdf),
                                                                 GetCompleteFilesCount(completeFiles, FileExtensionType.Dwg),
                                                                 GetCompleteFilesCount(completeFiles, FileExtensionType.Xlsx)));
        /// <summary>
        /// Получить количество обработанных файлов по типу 
        /// </summary>
        private static int GetCompleteFilesCount(IEnumerable<FileExtensionCount> completeFiles, FileExtensionType fileExtensionType) =>
            completeFiles.FirstOrDefault(completeFile => completeFile.FileExtensionType == fileExtensionType)?.Count ?? 0;
    }
}