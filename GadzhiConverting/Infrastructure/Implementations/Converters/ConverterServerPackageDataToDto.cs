using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public class ConverterServerPackageDataToDto : IConverterServerPackageDataToDto
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterServerPackageDataToDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public PackageDataShortResponseServer FilesDataToShortResponse(IPackageServer packageServer) =>
           new PackageDataShortResponseServer
           {
               Id = packageServer.Id,
               StatusProcessingProject = packageServer.StatusProcessingProject,
               FilesData = packageServer.FilesDataServer?.Select(FileDataToIntermediateResponse).ToList(),
           };

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        public async Task<PackageDataResponseServer> FilesDataToResponse(IPackageServer packageServer)
        {
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
        private static FileDataShortResponseServer FileDataToIntermediateResponse(IFileDataServer fileDataServer) =>
            new FileDataShortResponseServer
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileErrors = fileDataServer.FileErrors.Select(ToErrorCommon).ToList(),
            };

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        public async Task<FileDataResponseServer> FileDataToResponse(IFileDataServer fileDataServer)
        {
            var filesDataSourceTasks = fileDataServer.FilesDataSourceServer.Select(FileDataSourceToResponse);
            var filesDataSource = await Task.WhenAll(filesDataSourceTasks);
            return new FileDataResponseServer
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FilesDataSource = filesDataSource.Select(resultSource => resultSource.Value).ToList(),
                FileErrors = fileDataServer.FileErrors.Concat(filesDataSource.SelectMany(resultSource => resultSource.Errors)).
                                            Select(ToErrorCommon).ToList(),
            };
        }

        /// <summary>
        /// Конвертировать список отконвертированных файлов в окончательный ответ
        /// </summary>
        private async Task<IResultValue<FileDataSourceResponseServer>> FileDataSourceToResponse(IFileDataSourceServer fileDataSourceServer) =>
            await _fileSystemOperations.FileToByteAndZip(fileDataSourceServer.FilePathServer).
            MapAsync(resultFile =>
                new FileDataSourceResponseServer
                {
                    FileName = fileDataSourceServer.FileNameClient,
                    FileExtensionType = fileDataSourceServer.FileExtensionType,
                    PaperSizes = fileDataSourceServer.PaperSizes.Select(paperSize => paperSize.ToString()).ToList(),
                    PrinterName = fileDataSourceServer.PrinterName,
                    FileDataSource = resultFile.Value,
                }.Map(fileSource => new ResultValue<FileDataSourceResponseServer>(fileSource, resultFile.Errors)));

        /// <summary>
        /// Преобразовать ошибку в трансферную модель
        /// </summary> 
        private static ErrorCommonResponse ToErrorCommon(IErrorCommon error) =>
          new ErrorCommonResponse
          {
              ErrorConvertingType = error.ErrorConvertingType,
              ErrorDescription = error.Description,
          };
    }
}