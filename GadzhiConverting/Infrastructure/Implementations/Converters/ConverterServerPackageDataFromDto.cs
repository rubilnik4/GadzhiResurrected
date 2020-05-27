using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
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
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        public async Task<IPackageServer> ToFilesDataServerAndSaveFile(PackageDataRequestServer packageDataRequest)
        {
            if (packageDataRequest == null) throw new ArgumentNullException(nameof(packageDataRequest));

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
        private static IConvertingSettings ToConvertingSettings(ConvertingSettingsRequest convertingSettingsRequest) =>
            (convertingSettingsRequest != null)
                ? new ConvertingSettings(convertingSettingsRequest.PersonId, convertingSettingsRequest.PdfNamingType)
                : throw new ArgumentNullException(nameof(convertingSettingsRequest));

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс
        /// </summary>      
        private async Task<IFileDataServer> ToFileDataServerAndSaveFile(FileDataRequestServer fileDataRequest, string packageGuid)
        {
            var fileSavedCheck = await SaveFileFromDtoRequest(fileDataRequest, packageGuid);

            return new FileDataServer(fileSavedCheck.FilePath, fileDataRequest.FilePath, fileDataRequest.ColorPrint,
                                      StatusProcessing.InQueue, fileSavedCheck.Errors);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileSavedCheck> SaveFileFromDtoRequest(FileDataRequestServer fileDataRequest, string packageGuid)
        {
            (bool isValid, var errorsFromValidation) = ValidateDtoData.IsFileDataRequestValid(fileDataRequest);
            if (!isValid) return new FileSavedCheck(errorsFromValidation);

            string directoryPath = _fileSystemOperations.CreateFolderByName(ProjectSettings.ConvertingDirectory, packageGuid);
            string filePath = Path.Combine(directoryPath, Guid.NewGuid() + Path.GetExtension(fileDataRequest.FilePath));
            if (String.IsNullOrWhiteSpace(directoryPath) ||
                !await _fileSystemOperations.UnzipFileAndSave(filePath, fileDataRequest.FileDataSource))
            {
                return new FileSavedCheck(FileConvertErrorType.RejectToSave);
            }

            return new FileSavedCheck(filePath);
        }
    }
}
