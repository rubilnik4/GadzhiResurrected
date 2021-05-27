using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
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
        public ConverterServerPackageDataToDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public PackageDataShortResponseServer FilesDataToShortResponse(IPackageServer packageServer) =>
           new PackageDataShortResponseServer(packageServer.Id, packageServer.StatusProcessingProject,
                                              packageServer.FilesDataServer.Select(FileDataToIntermediateResponse).ToList());

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        public async Task<PackageDataResponseServer> FilesDataToResponse(IPackageServer packageServer) =>
            new PackageDataResponseServer(packageServer.Id, packageServer.StatusProcessingProject,
                                          await packageServer.FilesDataServer.Select(FileDataToResponse).
                                          Map(Task.WhenAll));

        /// <summary>
        /// Конвертировать файл серверной модели в промежуточную
        /// </summary>
        private static FileDataShortResponseServer FileDataToIntermediateResponse(IFileDataServer fileDataServer) =>
            new FileDataShortResponseServer(fileDataServer.FilePathClient, fileDataServer.StatusProcessing,
                                            fileDataServer.FileErrors.Select(ToErrorCommon).ToList());

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        public async Task<FileDataResponseServer> FileDataToResponse(IFileDataServer fileDataServer) =>
            await fileDataServer.FilesDataSourceServer.Select(FileDataSourceToResponse).
            Map(Task.WhenAll).
            MapAsync(filesDataSource =>
                 new FileDataResponseServer(fileDataServer.FilePathClient, fileDataServer.StatusProcessing,
                                            fileDataServer.FileErrors.Concat(filesDataSource.SelectMany(resultSource => resultSource.Errors)).
                                                            Select(ToErrorCommon).ToList(),
                                            filesDataSource.Select(resultSource => resultSource.Value).ToList()));

        /// <summary>
        /// Конвертировать список отконвертированных файлов в окончательный ответ
        /// </summary>
        private async Task<IResultValue<FileDataSourceResponseServer>> FileDataSourceToResponse(IFileDataSourceServer fileDataSourceServer) =>
            await _fileSystemOperations.FileToByteAndZip(fileDataSourceServer.FilePathServer).
            MapAsync(resultFile =>
                new FileDataSourceResponseServer(fileDataSourceServer.FileNameClient, fileDataSourceServer.FileExtensionType,
                                                 fileDataSourceServer.PaperSize.ToString(),
                                                 fileDataSourceServer.PrinterName, resultFile.Value).
                Map(fileSource => new ResultValue<FileDataSourceResponseServer>(fileSource, resultFile.Errors)));

        /// <summary>
        /// Преобразовать ошибку в трансферную модель
        /// </summary> 
        private static ErrorCommonResponse ToErrorCommon(IErrorCommon error) =>
          new ErrorCommonResponse(error.ErrorConvertingType, error.Description);
    }
}