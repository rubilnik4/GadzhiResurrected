using GadzhiCommon.Enums.FilesConvert;
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
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;
using GadzhiDTOServer.Infrastructure.Implementation;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в серверную
    /// </summary>      
    public class ConverterServerPackageDataFromDto : IConverterServerPackageDataFromDto
    {
        public ConverterServerPackageDataFromDto(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        public async Task<IResultValue<IPackageServer>> ToFilesDataServerAndSaveFile(PackageDataRequestServer packageDataRequest) =>
             await packageDataRequest.FilesData.
             Select(fileData => ToFileDataServerAndSaveFile(fileData, packageDataRequest.Id.ToString())).
             Map(Task.WhenAll).
             MapAsync(results => results.ToResultCollection()).
             MapAsync(result => (IResultValue<IReadOnlyCollection<IFileDataServer>>)result).
             ResultValueOkAsync(filesData => new PackageServer(packageDataRequest.Id, packageDataRequest.AttemptingConvertCount,
                                     StatusProcessingProject.Converting,
                                     ToConvertingSettings(packageDataRequest.ConvertingSettings),
                                     filesData));


        /// <summary>
        /// Преобразовать параметры конвертации из трансферной модели
        /// </summary>
        private static IConvertingPackageSettings ToConvertingSettings(ConvertingSettingsRequest convertingSettingsRequest) =>
            new ConvertingPackageSettings(convertingSettingsRequest.PersonId, convertingSettingsRequest.PdfNamingType,
                                   convertingSettingsRequest.ConvertingModeTypes, convertingSettingsRequest.UseDefaultSignature);

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс
        /// </summary>      
        private async Task<IResultValue<IFileDataServer>> ToFileDataServerAndSaveFile(FileDataRequestServer fileDataRequest, string packageGuid) =>
             await SaveFileFromDtoRequest(fileDataRequest, packageGuid).
             ResultValueOkAsync(filePath => new FileDataServer(filePath, fileDataRequest.FilePath, fileDataRequest.ColorPrintType,
                                                               StatusProcessing.InQueue, new List<IErrorCommon>()));

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<IResultValue<string>> SaveFileFromDtoRequest(FileDataRequestServer fileDataRequest, string packageGuid) =>
             await ValidateDtoData.IsFileDataRequestValid(fileDataRequest).
             ToResultValue(_fileSystemOperations.CreateFolderByName(ProjectSettings.ConvertingDirectory, packageGuid)).
             ResultValueOk(directoryPath => (directoryPath, Guid.NewGuid())).
             ResultValueOkBindAsync(directoryGuid =>
                SaveFile(fileDataRequest, directoryGuid.directoryPath, directoryGuid.Item2).
                MapBindAsync(result => SaveAdditionalFileIsNeed(fileDataRequest, directoryGuid.directoryPath, directoryGuid.Item2).
                                       MapAsync(resultAdditional => result.ConcatErrors(resultAdditional.Errors))));

        /// <summary>
        /// Сохранить файл
        /// </summary>
        private async Task<IResultValue<string>> SaveFile(FileDataRequestServer fileDataRequest, string directoryPath,
                                                          Guid guidPackage) =>
            await Path.Combine(directoryPath, guidPackage + Path.GetExtension(fileDataRequest.FilePath)).
            Map(filePath => _fileSystemOperations.UnzipFileAndSave(filePath, fileDataRequest.FileDataSource));

        /// <summary>
        /// Сохранить дополнительный файл
        /// </summary>
        private async Task<IResultError> SaveAdditionalFileIsNeed(FileDataRequestServer fileDataRequest, string directoryPath,
                                                            Guid guidPackage) =>
            await fileDataRequest.FileExtensionAdditional.
            WhereContinueAsyncBind(filePath => !String.IsNullOrWhiteSpace(fileDataRequest.FileExtensionAdditional),
                okFunc: _ => SaveAdditionalFile(fileDataRequest, directoryPath, guidPackage).
                             MapAsync(result => result.ToResult()),
                badFunc: _ => Task.FromResult((IResultError)new ResultError()));

        /// <summary>
        /// Сохранить дополнительный файл
        /// </summary>
        private async Task<IResultValue<string>> SaveAdditionalFile(FileDataRequestServer fileDataRequest, string directoryPath,
                                                                    Guid guidPackage) =>
            await FilePathOperations.CombineFilePath(directoryPath, guidPackage.ToString(), fileDataRequest.FileExtensionAdditional).
            WhereContinueAsyncBind(_ => fileDataRequest.FileDataSourceAdditional != null &&
                                        fileDataRequest.FileDataSourceAdditional.Length > 0,
                okFunc: filePath => _fileSystemOperations.UnzipFileAndSave(filePath, fileDataRequest.FileDataSourceAdditional),
                badFunc: filePath => new ResultValue<string>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                      $"Файл дополнительный данных не задан {filePath}")).
                              Map(result => Task.FromResult((IResultValue<string>)result)));
    }
}
