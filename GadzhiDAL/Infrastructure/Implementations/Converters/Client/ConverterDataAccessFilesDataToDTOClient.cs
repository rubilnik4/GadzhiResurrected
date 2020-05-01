using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Client
{
    /// <summary>
    /// Конвертер из модели базы данных в трансферную для клиента
    /// </summary>
    public class ConverterDataAccessFilesDataToDTOClient : IConverterDataAccessFilesDataToDTOClient
    {
        public ConverterDataAccessFilesDataToDTOClient()
        {

        }

        /// <summary>
        /// Конвертировать из модели базы данных в промежуточную
        /// </summary>       
        public async Task<PackageDataIntermediateResponseClient> ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                                                            FilesQueueInfo filesQueueInfo)
        {
            if (filesDataEntity == null) return null;

            var filesDataTasks = filesDataEntity.FileDataEntities?.AsQueryable()?.
                                 Select(fileData => ConvertFileDataAccessToIntermediateResponse(fileData));
            var filesData = await Task.WhenAll(filesDataTasks);

            return new PackageDataIntermediateResponseClient()
            {
                Id = Guid.Parse(filesDataEntity.Id),
                StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                FileDatas = filesData,
                FilesQueueInfo = ConvertFilesQueueInfoToResponse(filesQueueInfo),
            };
        }

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public async Task<PackageDataResponseClient> ConvertFilesDataAccessToResponse(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity == null) return null;

            var filesDataTasks = filesDataEntity.FileDataEntities?.AsQueryable()?.
                                 Select(fileData => ConvertFileDataAccessToResponse(fileData));
            var filesData = await Task.WhenAll(filesDataTasks);

            return new PackageDataResponseClient()
            {
                Id = Guid.Parse(filesDataEntity.Id),
                StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                FilesData = filesData,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private async Task<FileDataIntermediateResponseClient> ConvertFileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileConvertErrorType = await fileDataEntity.FileConvertErrorType.AsQueryable().ToListAsync();
            if (!CheckStatusProcessing.CompletedStatusProcessingServer.Contains(fileDataEntity.StatusProcessing) &&
                !fileDataEntity.FileConvertErrorType.Any())
            {
                fileConvertErrorType = new List<FileConvertErrorType> { FileConvertErrorType.UnknownError };
            }

            return new FileDataIntermediateResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileConvertErrorTypes = fileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        private async Task<FileDataResponseClient> ConvertFileDataAccessToResponse(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileDataSourceResponseClient = await fileDataEntity.FileDataSourceServerEntities.AsQueryable().
                                               Select(fileData => ConvertFileDataSourceResponse(fileData)).ToListAsync();

            return new FileDataResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileDatasSourceResponseClient = fileDataSourceResponseClient,
                FileConvertErrorTypes = fileDataEntity.FileConvertErrorType.AsQueryable().ToList(),
            };
        }

        /// <summary>
        /// Конвертировать информацию о количестве файлов в очереди
        /// </summary>        
        private FilesQueueInfoResponseClient ConvertFilesQueueInfoToResponse(FilesQueueInfo filesQueueInfo) =>
            new FilesQueueInfoResponseClient()
            {
                FilesInQueueCount = filesQueueInfo?.FilesInQueueCount ?? 0,
                PackagesInQueueCount = filesQueueInfo?.PackagesInQueueCount ?? 0,
            };

        /// <summary>
        /// Конвертировать информацию о готовых файлах
        /// </summary>        
        private FileDataSourceResponseClient ConvertFileDataSourceResponse(FileDataSourceEntity fileDataSourceEntity) =>
            new FileDataSourceResponseClient()
            {
                FileName = fileDataSourceEntity?.FileName,
                FileDataSource = fileDataSourceEntity?.FileDataSource?.AsQueryable().ToList(),
            };
    }
}