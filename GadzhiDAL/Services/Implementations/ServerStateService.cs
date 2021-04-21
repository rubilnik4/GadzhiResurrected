using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Entities.Signatures;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.DataFile;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOBase.TransferModels.ServerStates;
using NHibernate.Linq;
using Unity;

namespace GadzhiDAL.Services.Implementations
{
    /// <summary>
    /// Получение информации о серверах в БД
    /// </summary>
    public class ServerStateService: IServerStateService
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

            var countSourceArchive = await unitOfWork.Session.Query<FileDataSourceArchiveEntity>().
                                                 GroupBy(entity => entity.FileExtensionType).
                                                 Select(group => new {Key = group.Key, Count = group.Count() }).
                                                 ToListAsync();

            var countSource =await unitOfWork.Session.Query<FileDataSourceEntity>().
                                                 GroupBy(entity => entity.FileExtensionType).
                                                 Select(group => new { Key = group.Key, Count = group.Count() }).
                                                 ToListAsync();

            //var count = countArchiveSource.Join(countSource,
            //                                    first => first.Key,
            //                                    second => second.Key,
            //                                    (first, second) => new { first.Key, Count = first.Count + second.Count }).
            //                                ToList();


            //var dgnCount = await unitOfWork.Session.Query<FileDataSourceArchiveEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Dgn).
            //                                        CountAsync() +
            //               await unitOfWork.Session.Query<FileDataSourceEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Dgn).
            //                                        CountAsync();

            //var docCount = await unitOfWork.Session.Query<FileDataSourceArchiveEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Doc ||
            //                                                      entity.FileExtensionType == FileExtensionType.Docx).
            //                                        CountAsync() +
            //               await unitOfWork.Session.Query<FileDataSourceEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Doc ||
            //                                                      entity.FileExtensionType == FileExtensionType.Docx).
            //                                        CountAsync();

            //var pdfCount = await unitOfWork.Session.Query<FileDataSourceArchiveEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Pdf).
            //                                        CountAsync() +
            //               await unitOfWork.Session.Query<FileDataSourceEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Pdf).
            //                                        CountAsync();

            //var dwgCount = await unitOfWork.Session.Query<FileDataSourceArchiveEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Dwg).
            //                                        CountAsync() +
            //               await unitOfWork.Session.Query<FileDataSourceEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Dwg).
            //                                        CountAsync();
            //var xlsCount = await unitOfWork.Session.Query<FileDataSourceArchiveEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Xlsx).
            //                                        CountAsync() +
            //               await unitOfWork.Session.Query<FileDataSourceEntity>().
            //                                        Where(entity => entity.FileExtensionType == FileExtensionType.Xlsx).
            //                                        CountAsync();

            return new ServerCompleteFilesResponse(0, 0, 0, 0, 0);
        }
    }
}