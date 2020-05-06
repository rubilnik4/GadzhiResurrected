using System;
using System.Collections.Generic;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System.Linq;
using System.Threading.Tasks;
using GadzhiModules.Modules.FilesConvertModule.Models.Interfaces;

namespace GadzhiModules.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертеры из локальной модели в трансферную
    /// </summary>  
    public class ConverterClientPackageDataToDto : IConverterClientPackageDataToDto
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterClientPackageDataToDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Конвертер пакета информации о файле из локальной модели в трансферную
        /// </summary>      
        public async Task<PackageDataRequestClient> ToPackageDataRequest(IPackageData packageData)
        {
            if (packageData == null) throw new ArgumentNullException(nameof(packageData));

            var filesRequestExistTask = packageData.FilesData?.
                                        Where(file => _fileSystemOperations.IsFileExist(file.FilePath)).
                                        Select(ToFileDataRequest)
                                        ?? Enumerable.Empty<Task<(bool, FileDataRequestClient)>>();

            var filesRequestExist = await Task.WhenAll(filesRequestExistTask);
            var filesRequestEnsuredWithBytes = filesRequestExist.
                                               Where(fileSuccess => fileSuccess.Success).
                                               Select(fileSuccess => fileSuccess.FileDataSourceRequest);

            return new PackageDataRequestClient()
            {
                Id = packageData.GenerateId(),
                FilesData = filesRequestEnsuredWithBytes.ToList(),
            };
        }

        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        private async Task<(bool Success, FileDataRequestClient FileDataSourceRequest)> ToFileDataRequest(FileData fileData)
        {
            (bool success, var fileDataSourceZip) = await _fileSystemOperations.FileToByteAndZip(fileData.FilePath);
            var fileDataRequestClient = new FileDataRequestClient()
            {
                ColorPrint = fileData.ColorPrint,
                FilePath = fileData.FilePath,
                StatusProcessing = fileData.StatusProcessing,
                FileDataSource = fileDataSourceZip,
            };
            return (success, fileDataRequestClient);
        }
    }
}
