using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
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
                                                                      ConvertFileToIntermediateResponse(fileDataServer)),
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
                FileConvertErrorType = fileDataServer.FileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        private async Task<FileDataResponseServer> ConvertFileResponse(FileDataServer fileDataServer)
        {
            var fileDataSource = await _fileSystemOperations.ConvertFileToByteAndZip(fileDataServer.FilePathServer);

            return new FileDataResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                IsCompleted = fileDataServer.IsCompleted,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileDataSource = fileDataSource,
                FileConvertErrorType = fileDataServer.FileConvertErrorType,
            };
        }
    }
}