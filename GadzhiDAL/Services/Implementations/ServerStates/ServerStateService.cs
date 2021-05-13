using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiDAL.Entities.FilesConvert;
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
            var countSource = await GetServerSourceCompleteFiles(unitOfWork);

            return GetServerCompleteFiles(countSource);
        }

        /// <summary>
        /// Получить информацию о обработанных файлах из архива и текущего списка
        /// </summary>
        private static async Task<IReadOnlyCollection<FileExtensionCount>> GetServerSourceCompleteFiles(IUnitOfWork unitOfWork)=>
            await unitOfWork.Session.
            Query<FileDataSourceEntity>().
            GroupBy(entity => entity.FileExtensionType).
            Select(group => new { Key = group.Key, Count = group.Count() }).
            ToListAsync().
            MapAsync(completeFiles => completeFiles.Select(completeFile => new FileExtensionCount(completeFile.Key, completeFile.Count)).ToList());

        /// <summary>
        /// Получить информацию о обработанных файлах
        /// </summary>
        private static ServerCompleteFilesResponse GetServerCompleteFiles(IReadOnlyCollection<FileExtensionCount> sourceCount) =>
            new ServerCompleteFilesResponse(GetCompleteFilesCount(sourceCount, FileExtensionType.Dgn),
                                                                 GetCompleteFilesCount(sourceCount, FileExtensionType.Doc) +
                                                                 GetCompleteFilesCount(sourceCount, FileExtensionType.Docx),
                                                                 GetCompleteFilesCount(sourceCount, FileExtensionType.Pdf),
                                                                 GetCompleteFilesCount(sourceCount, FileExtensionType.Dwg),
                                                                 GetCompleteFilesCount(sourceCount, FileExtensionType.Xlsx));
        /// <summary>
        /// Получить количество обработанных файлов по типу 
        /// </summary>
        private static int GetCompleteFilesCount(IEnumerable<FileExtensionCount> completeFiles, FileExtensionType fileExtensionType) =>
            completeFiles.FirstOrDefault(completeFile => completeFile.FileExtensionType == fileExtensionType)?.Count ?? 0;
    }
}