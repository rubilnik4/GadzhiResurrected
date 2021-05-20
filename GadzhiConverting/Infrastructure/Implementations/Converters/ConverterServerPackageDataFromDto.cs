using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces.Printers;
using GadzhiDTOServer.Infrastructure.Implementation;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в серверную
    /// </summary>      
    public class ConverterServerPackageDataFromDto : IConverterServerPackageDataFromDto
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterServerPackageDataFromDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        public async Task<IPackageServer> ToFilesDataServerAndSaveFile(PackageDataRequestServer packageDataRequest)
        {
            var filesDataServerToConvertTask = packageDataRequest.FilesData?.Select(fileData =>
                                               ToFileDataServerAndSaveFile(fileData, packageDataRequest.Id.ToString()));
            var filesDataServerToConvert = await Task.WhenAll(filesDataServerToConvertTask ?? new List<Task<IFileDataServer>>());

            return new PackageServer(packageDataRequest.Id, packageDataRequest.AttemptingConvertCount,
                                     StatusProcessingProject.Converting,
                                     ToConvertingSettings(packageDataRequest.ConvertingSettings),
                                     filesDataServerToConvert);
        }

        /// <summary>
        /// Преобразовать параметры конвертации из трансферной модели
        /// </summary>
        private static IConvertingPackageSettings ToConvertingSettings(ConvertingSettingsRequest convertingSettingsRequest) => 
            new ConvertingPackageSettings(convertingSettingsRequest.PersonId, convertingSettingsRequest.PdfNamingType,
                                   convertingSettingsRequest.ConvertingModeTypes, convertingSettingsRequest.UseDefaultSignature);

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс
        /// </summary>      
        private async Task<IFileDataServer> ToFileDataServerAndSaveFile(FileDataRequestServer fileDataRequest, string packageGuid)
        {
            var fileSavedCheck = await SaveFileFromDtoRequest(fileDataRequest, packageGuid);
            return new FileDataServer(fileSavedCheck.FilePath, fileDataRequest.FilePath, fileDataRequest.ColorPrintType,
                                      StatusProcessing.InQueue, fileSavedCheck.Errors);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileSavedCheck> SaveFileFromDtoRequest(FileDataRequestServer fileDataRequest, string packageGuid)
        {
            var errorsFromValidation = ValidateDtoData.IsFileDataRequestValid(fileDataRequest);
            if (errorsFromValidation.Count > 0) return new FileSavedCheck(errorsFromValidation);

            string directoryPath = _fileSystemOperations.CreateFolderByName(ProjectSettings.ConvertingDirectory, packageGuid);
            var fileGuid = Guid.NewGuid();
            string filePath = Path.Combine(directoryPath, fileGuid + Path.GetExtension(fileDataRequest.FilePath));
            if (String.IsNullOrWhiteSpace(directoryPath) ||
                !await _fileSystemOperations.UnzipFileAndSave(filePath, fileDataRequest.FileDataSource))
            {
                return new FileSavedCheck(new ErrorCommon(ErrorConvertingType.RejectToSave, $"Невозможно сохранить файл {filePath}"));
            }
            await SaveAdditionalFile(fileDataRequest, directoryPath, fileGuid);

            return new FileSavedCheck(filePath);
        }

        /// <summary>
        /// Сохранить дополнительный файл
        /// </summary>
        private async Task SaveAdditionalFile(FileDataRequestServer fileDataRequest, string directoryPath, Guid guidPackage)
        {
            if (!String.IsNullOrWhiteSpace(fileDataRequest.FileExtensionAdditional) &&
              fileDataRequest.FileDataSourceAdditional != null &&
              fileDataRequest.FileDataSourceAdditional.Length > 0)
            {
                var additionalFilePath = FileSystemOperations.CombineFilePath(directoryPath, guidPackage.ToString(),
                                                                              fileDataRequest.FileExtensionAdditional);
                await _fileSystemOperations.UnzipFileAndSave(additionalFilePath, fileDataRequest.FileDataSourceAdditional);
            }
        }
    }
}
