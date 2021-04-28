using System;
using System.Collections.Generic;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

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
        public async Task<PackageDataRequestClient> ToPackageDataRequest(IPackageData packageData, IConvertingSettings convertingSetting)
        {
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
                ConvertingSettings = ToConvertingSettingsRequest(convertingSetting),
            };
        }

        /// <summary>
        /// Преобразовать параметры конвертации в трансферную модель
        /// </summary>
        private static ConvertingSettingsRequest ToConvertingSettingsRequest(IConvertingSettings convertingSetting) =>
             new ConvertingSettingsRequest
             {
                 PersonId = convertingSetting.PersonSignature.PersonId ?? String.Empty,
                 PdfNamingType = convertingSetting.PdfNamingType,
                 ConvertingModeType = convertingSetting.ConvertingModeType,
             };

        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        private async Task<(bool Success, FileDataRequestClient FileDataSourceRequest)> ToFileDataRequest(IFileData fileData)
        {
            (bool success, var fileDataSourceZip) = await _fileSystemOperations.FileToByteAndZip(fileData.FilePath);

            var filePathAdditional = AdditionalFileExtensions.FileExtensions.
                                     Select(extension => FileSystemOperations.ChangeFileName(fileData.FilePath, fileData.FileName,
                                                                                             extension)).
                                     FirstOrDefault(filePath => _fileSystemOperations.IsFileExist(filePath));
            (bool _, var fileDataSourceZipAdditional) = !String.IsNullOrWhiteSpace(filePathAdditional) 
                    ? await _fileSystemOperations.FileToByteAndZip(filePathAdditional)
                    : (false, null);

            var fileDataRequestClient = new FileDataRequestClient
            {
                ColorPrintType = fileData.ColorPrintType,
                FilePath = fileData.FilePath,
                StatusProcessing = fileData.StatusProcessing,
                FileDataSource = fileDataSourceZip,
                FileExtensionAdditional = FileSystemOperations.ExtensionWithoutPointFromPath(filePathAdditional),
                FileDataSourceAdditional = fileDataSourceZipAdditional,
            };
            return (success, fileDataRequestClient);
        }
    }
}
