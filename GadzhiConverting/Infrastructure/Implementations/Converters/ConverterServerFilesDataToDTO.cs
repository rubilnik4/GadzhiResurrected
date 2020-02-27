using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public class ConverterServerFilesDataToDTO : IConverterServerFilesDataToDTO
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterServerFilesDataToDTO(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public FilesDataIntermediateResponseServer ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer)
        {
            if (filesDataServer != null)
            {
                return new FilesDataIntermediateResponseServer()
                {
                    Id = filesDataServer.Id,
                    IsCompleted = filesDataServer.IsCompleted,
                    StatusProcessingProject = filesDataServer.StatusProcessingProject,
                    FilesData = filesDataServer.FilesDataInfo?.Select(fileDataServer =>
                                ConvertFileToIntermediateResponse(fileDataServer)).ToList(),
                };
            }
            else
            {
                throw new ArgumentNullException(nameof(filesDataServer));
            }
        }

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        public async Task<FilesDataResponseServer> ConvertFilesToResponse(FilesDataServer filesDataServer)
        {
            var filesDataToResponseTasks = filesDataServer?.FilesDataInfo?.Select(fileDataServer =>
                                                           ConvertFileResponse(fileDataServer));
            var filesDataToResponse = await Task.WhenAll(filesDataToResponseTasks);

            return new FilesDataResponseServer()
            {
                Id = filesDataServer.Id,
                IsCompleted = filesDataServer.IsCompleted,
                StatusProcessingProject = filesDataServer.StatusProcessingProject,
                FilesData = filesDataToResponse,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в промежуточную
        /// </summary>
        private FileDataIntermediateResponseServer ConvertFileToIntermediateResponse(FileDataServer fileDataServer)
        {
            return new FileDataIntermediateResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                IsCompleted = fileDataServer.IsCompleted,
                FileConvertErrorType = fileDataServer.FileConvertErrorTypes,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        private async Task<FileDataResponseServer> ConvertFileResponse(FileDataServer fileDataServer)
        {
            var filesDataSourceTasks = fileDataServer.FileDataSourceServer?.Select(fileData => ConvertFileDataSourceResponse(fileData));
            var filesDataSource = await Task.WhenAll(filesDataSourceTasks);

            return new FileDataResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                IsCompleted = fileDataServer.IsCompleted,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileDataSourceResponseServer = filesDataSource,
                FileConvertErrorType = fileDataServer.FileConvertErrorTypes,
            };
        }

        /// <summary>
        /// Конвертировать список отконвертированных файлов в окончательный ответ
        /// </summary>
        private async Task<FileDataSourceResponseServer> ConvertFileDataSourceResponse(FileDataSourceServer fileDataSourceServer)
        {
            var fileDataSource = await _fileSystemOperations.ConvertFileToByteAndZip(fileDataSourceServer.FilePath);

            return new FileDataSourceResponseServer()
            {
                FileName = Path.GetFileName(fileDataSourceServer.FilePath),
                FileDataSource = fileDataSource,
            };
        }
    }
}