using System;
using System.Collections.Generic;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
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
        public ConverterClientPackageDataToDto(IFileSystemOperations fileSystemOperations, IFilePathOperations filePathOperations)
        {
            _fileSystemOperations = fileSystemOperations;
            _filePathOperations = filePathOperations;
            ;
        }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Операции с путями файлов
        /// </summary>
        private readonly IFilePathOperations _filePathOperations;

        /// <summary>
        /// Конвертер пакета информации о файле из локальной модели в трансферную
        /// </summary>      
        public async Task<IResultValue<PackageDataRequestClient>> ToPackageDataRequest(IPackageData packageData,
                                                                                       IConvertingSettings convertingSetting)
        {
            var filesRequestTask = packageData.FilesData.Select(ToFileDataRequest);
            var filesRequest = await Task.WhenAll(filesRequestTask);
            return filesRequest.ToResultCollection().
                   ResultValueOk(requests => new PackageDataRequestClient
                   {
                       Id = packageData.GenerateId(),
                       FilesData = requests.ToList(),
                       ConvertingSettings = ToConvertingSettingsRequest(convertingSetting),
                   });
        }

        /// <summary>
        /// Преобразовать параметры конвертации в трансферную модель
        /// </summary>
        private static ConvertingSettingsRequest ToConvertingSettingsRequest(IConvertingSettings convertingSetting) =>
             new ConvertingSettingsRequest(convertingSetting.PersonSignature.PersonId ?? String.Empty,
                                           convertingSetting.PdfNamingType, convertingSetting.ConvertingModesUsed.ToList(),
                                           convertingSetting.UseDefaultSignature);

        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        private async Task<IResultValue<FileDataRequestClient>> ToFileDataRequest(IFileData fileData) =>
            await _fileSystemOperations.FileToByteAndZip(fileData.FilePath).
            ResultValueOkBindAsync(fileSource =>
                AdditionalFileExtensions.FileExtensions.
                Select(extension => FilePathOperations.ChangeFileName(fileData.FilePath, fileData.FileName, extension)).
                FirstOrDefault(filePath => _filePathOperations.IsFileExist(filePath)).
                Map(fileAdditionalPath => ToFileDataRequest(fileData, fileSource, fileAdditionalPath)));

        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        private async Task<IResultValue<FileDataRequestClient>> ToFileDataRequest(IFileData fileData, byte[] fileSource,
                                                                                  string fileAdditionalPath) =>
            await fileAdditionalPath.
            WhereContinueAsyncBind(filePath => !String.IsNullOrWhiteSpace(filePath),
                filePath => _fileSystemOperations.FileToByteAndZip(filePath),
                filePath => Task.FromResult((IResultValue<byte[]>)new ResultValue<byte[]>(null))).
            ResultValueOkAsync(fileAdditionalSource => new FileDataRequestClient
            {
                ColorPrintType = fileData.ColorPrintType,
                FilePath = fileData.FilePath,
                StatusProcessing = fileData.StatusProcessing,
                FileDataSource = fileSource,
                FileExtensionAdditional = FilePathOperations.ExtensionWithoutPointFromPath(fileAdditionalPath),
                FileDataSourceAdditional = fileAdditionalSource,
            });
    }
}
