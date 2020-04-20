using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
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
        public FilesDataIntermediateResponseServer ConvertFilesToIntermediateResponse(IFilesDataServer filesDataServer)
        {
            if (filesDataServer != null)
            {
                return new FilesDataIntermediateResponseServer()
                {
                    Id = filesDataServer.Id,
                    StatusProcessingProject = filesDataServer.StatusProcessingProject,
                    FileDatas = filesDataServer.FileDatasServer?.Select(fileDataServer =>
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
        public async Task<FilesDataResponseServer> ConvertFilesToResponse(IFilesDataServer filesDataServer)
        {
            var filesDataToResponseTasks = filesDataServer?.FileDatasServer?.Select(fileDataServer =>
                                                            ConvertFileResponse(fileDataServer));
            var filesDataToResponse = await Task.WhenAll(filesDataToResponseTasks);

            return new FilesDataResponseServer()
            {
                Id = filesDataServer.Id,
                StatusProcessingProject = filesDataServer.StatusProcessingProject,
                FileDatas = filesDataToResponse,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в промежуточную
        /// </summary>
        private FileDataIntermediateResponseServer ConvertFileToIntermediateResponse(IFileDataServer fileDataServer)
        {
            return new FileDataIntermediateResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileConvertErrorType = fileDataServer.FileConvertErrorTypes.ToList(),
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        private async Task<FileDataResponseServer> ConvertFileResponse(IFileDataServer fileDataServer)
        {
            var filesDataSourceTasks = fileDataServer.FileDatasSourceServer?.Select(fileData => ConvertFileDataSourceResponse(fileData));
            var filesDataSource = await Task.WhenAll(filesDataSourceTasks);

            return new FileDataResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileDatasSourceResponseServer = filesDataSource,
                FileConvertErrorType = fileDataServer.FileConvertErrorTypes.ToList(),
            };
        }

        /// <summary>
        /// Конвертировать список отконвертированных файлов в окончательный ответ
        /// </summary>
        private async Task<FileDataSourceResponseServer> ConvertFileDataSourceResponse(IFileDataSourceServer fileDataSourceServer)
        {
            var fileDataSource = await _fileSystemOperations.ConvertFileToByteAndZip(fileDataSourceServer.FilePath);

            return new FileDataSourceResponseServer()
            {
                FileName = Path.GetFileName(fileDataSourceServer.FilePath),
                PaperSize = fileDataSourceServer.PaperSize,
                PrinterName = fileDataSourceServer.PrinterName,
                FileDataSource = fileDataSource,
            };
        }
    }
}