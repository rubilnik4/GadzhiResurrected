using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
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
    public class ConverterServerFilesDataToDto : IConverterServerFilesDataToDto
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterServerFilesDataToDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public PackageDataIntermediateResponseServer FilesDataToIntermediateResponse(IPackageServer packageServer) =>
            (packageServer != null)
                ? new PackageDataIntermediateResponseServer()
                {
                    Id = packageServer.Id,
                    StatusProcessingProject = packageServer.StatusProcessingProject,
                    FilesData = packageServer.FilesDataServer?.Select(FileDataToIntermediateResponse).ToList(),
                } 
                : throw new ArgumentNullException(nameof(packageServer));

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        public async Task<PackageDataResponseServer> FilesDataToResponse(IPackageServer packageServer)
        {
            if (packageServer == null) throw new ArgumentNullException(nameof(packageServer));

            var filesDataToResponseTasks = packageServer.FilesDataServer?.Select(FileDataToResponse)
                                           ?? Enumerable.Empty<Task<FileDataResponseServer>>();
            var filesDataToResponse = await Task.WhenAll(filesDataToResponseTasks);

            return new PackageDataResponseServer()
            {
                Id = packageServer.Id,
                StatusProcessingProject = packageServer.StatusProcessingProject,
                FilesData = filesDataToResponse,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в промежуточную
        /// </summary>
        private static FileDataIntermediateResponseServer FileDataToIntermediateResponse(IFileDataServer fileDataServer) =>
            new FileDataIntermediateResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileConvertErrorTypes = fileDataServer.FileConvertErrorTypes.ToList(),
            };

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        private async Task<FileDataResponseServer> FileDataToResponse(IFileDataServer fileDataServer)
        {
            var filesDataSourceTasks = fileDataServer.FilesDataSourceServer?.Select(FileDataSourceToResponse)
                                       ?? Enumerable.Empty<Task<(bool, FileDataSourceResponseServer)>>();
            var filesDataSource = await Task.WhenAll(filesDataSourceTasks);
            var filesDataSourceWithBytes = filesDataSource.
                                           Where(fileSuccess => fileSuccess.Success).
                                           Select(fileSuccess => fileSuccess.FileDataSourceResponse);

            return new FileDataResponseServer()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FilesDataSource = filesDataSourceWithBytes.ToList(),
                FileConvertErrorTypes = fileDataServer.FileConvertErrorTypes.ToList(),
            };
        }

        /// <summary>
        /// Конвертировать список отконвертированных файлов в окончательный ответ
        /// </summary>
        private async Task<(bool Success, FileDataSourceResponseServer FileDataSourceResponse)> FileDataSourceToResponse(IFileDataSourceServer fileDataSourceServer)
        {
            (bool success, var fileDataSourceZip) = await _fileSystemOperations.FileToByteAndZip(fileDataSourceServer.FilePath);

            var fileDataSourceResponseServer = new FileDataSourceResponseServer()
            {
                FileName = Path.GetFileName(fileDataSourceServer.FilePath),
                PaperSize = fileDataSourceServer.PaperSize,
                PrinterName = fileDataSourceServer.PrinterName,
                FileDataSource = fileDataSourceZip,
            };
            return (success, fileDataSourceResponseServer);
        }
    }
}