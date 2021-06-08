using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Infrastructure.Interfaces.Converters;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертеры из трансферной модели в локальную
    /// </summary>  
    public class ConverterClientPackageDataFromDto : IConverterClientPackageDataFromDto
    {
        public ConverterClientPackageDataFromDto(IFileSystemOperations fileSystemOperations, IDialogService dialogService)
        {
            _fileSystemOperations = fileSystemOperations;
            _dialogService = dialogService;
        }

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private readonly IDialogService _dialogService;

        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public PackageStatus ToPackageStatusFromIntermediateResponse(PackageDataShortResponseClient packageDataShortResponse) =>
            new PackageStatus(packageDataShortResponse.FilesData.Select(ToFileStatusFromIntermediateResponse),
                              packageDataShortResponse.StatusProcessingProject,
                              ConvertToQueueInfoFromResponse(packageDataShortResponse.FilesQueueInfo));

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        public PackageStatus ToPackageStatus(PackageDataResponseClient packageDataResponse) =>
            new PackageStatus(ToPackageStatusFromResponse(packageDataResponse), StatusProcessingProject.Writing);

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        public async Task<PackageStatus> ToFilesStatusAndSaveFiles(PackageDataResponseClient packageDataResponse) =>
            new PackageStatus(await ToPackageStatusFromResponseAndSaveFiles(packageDataResponse),
                              StatusProcessingProject.End);

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private static IEnumerable<FileStatus> ToPackageStatusFromResponse(PackageDataResponseClient packageDataResponse) =>
            packageDataResponse.FilesData?.Select(ConvertToFileStatusFromResponse);

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<IEnumerable<FileStatus>> ToPackageStatusFromResponseAndSaveFiles(PackageDataResponseClient packageDataResponse)
        {
            var filesStatusTask = packageDataResponse.FilesData?.Select(ToFileStatusFromResponseAndSaveFile);
            var filesStatus = await Task.WhenAll(filesStatusTask ?? Enumerable.Empty<Task<FileStatus>>());
            return filesStatus;
        }

        /// <summary>
        /// Конвертер информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        private static FileStatus ToFileStatusFromIntermediateResponse(FileDataShortResponseClient fileResponse) =>
            new FileStatus(fileResponse.FilePath, fileResponse.StatusProcessing,
                           fileResponse.FileErrors.Select(ToErrorCommon));

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        public static FileStatus ConvertToFileStatusFromResponse(FileDataResponseClient fileResponse) =>
             new FileStatus(fileResponse.FilePath, StatusProcessing.Writing,
                            fileResponse.FileErrors.Select(ToErrorCommon));

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        public async Task<FileStatus> ToFileStatusFromResponseAndSaveFile(FileDataResponseClient fileResponse)
        {
            var fileConvertSavedErrorType = await SaveFileDataSourceFromDtoResponse(fileResponse);
            var fileConvertErrorTypes = fileResponse.FileErrors.Select(ToErrorCommon).
                                        UnionNotNull(fileConvertSavedErrorType).
                                        Where(error => error.ErrorConvertingType != ErrorConvertingType.NoError);

            return new FileStatus(fileResponse.FilePath, StatusProcessing.End, fileConvertErrorTypes);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<IEnumerable<IErrorCommon>> SaveFileDataSourceFromDtoResponse(FileDataResponseClient fileDataResponse)
        {
            if (fileDataResponse.FilesDataSource == null)
                return new List<IErrorCommon> { new ErrorCommon(ErrorConvertingType.IncorrectDataSource, "Некорректные входные данные") };

            string fileDirectoryName = Path.GetDirectoryName(fileDataResponse.FilePath);
            string convertingDirectoryName = Path.Combine(fileDirectoryName ?? throw new InvalidOperationException(nameof(fileDirectoryName)),
                                                          ProjectSettings.DirectoryForSavingConvertedFiles);

            var fileConvertErrorTypeTasks = fileDataResponse.FilesDataSource?.
                                            Where(fileData => fileData.FileExtensionType != FileExtensionType.Print).
                                            Select(fileData => SaveFileDataSourceFromDtoResponse(fileData, convertingDirectoryName));
            return await Task.WhenAll(fileConvertErrorTypeTasks);
        }

        /// <summary>
        /// Сохранить файл из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<IErrorCommon> SaveFileDataSourceFromDtoResponse(FileDataSourceResponseClient fileDataSourceResponseClient,
                                                                           string convertingDirectoryName)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileDataSourceResponseClient.FileName);
            string fileExtension = FilePathOperations.ExtensionWithoutPoint(Path.GetExtension(fileDataSourceResponseClient.FileName));
            string fileExtensionValid = ValidFileExtensions.GetFileTypesValid(fileExtension).ToString().ToLowerCaseCurrentCulture();
            string directoryPath = _fileSystemOperations.CreateFolderByName(convertingDirectoryName, fileExtensionValid.ToUpperCaseCurrentCulture());


            if (String.IsNullOrWhiteSpace(fileName)) return new ErrorCommon(ErrorConvertingType.IncorrectFileName, $"Некорректное имя файла {fileName}");
            if (String.IsNullOrWhiteSpace(fileName)) return new ErrorCommon(ErrorConvertingType.IncorrectExtension, $"Некорректное расширение файла {fileExtension}");
            if (String.IsNullOrWhiteSpace(directoryPath)) return new ErrorCommon(ErrorConvertingType.RejectToSave, "Директория сохранения не создана");
            if (fileDataSourceResponseClient.FileDataSource.Length == 0) return new ErrorCommon(ErrorConvertingType.IncorrectDataSource,
                                                                                                $"Некорректные входные данные {fileName}");

            string filePath = FilePathOperations.CombineFilePath(directoryPath, fileName, fileExtensionValid);
            Task<bool> UnzipFileAndSaveBool() => _fileSystemOperations.UnzipFileAndSave(filePath, fileDataSourceResponseClient.FileDataSource).
                                                 MapAsync(result => result.OkStatus);
            await _dialogService.RetryOrIgnoreBoolFunction(UnzipFileAndSaveBool, $"Файл {filePath} открыт или используется. Повторить попытку сохранения?");
            return new ErrorCommon(ErrorConvertingType.NoError, "Ошибки отсутствуют");
        }
        /// <summary>
        /// Конвертер из трансферной модели информации в клиентскую
        /// </summary>       
        private static QueueStatus ConvertToQueueInfoFromResponse(FilesQueueInfoResponseClient filesQueueInfoResponse) =>
            new QueueStatus(filesQueueInfoResponse.FilesInQueueCount, filesQueueInfoResponse.PackagesInQueueCount);

        /// <summary>
        /// Конвертировать ошибку из трансферной модели
        /// </summary>
        private static IErrorCommon ToErrorCommon(ErrorCommonResponse errorCommonResponse) =>
            (errorCommonResponse != null)
                ? new ErrorCommon(errorCommonResponse.ErrorConvertingType, errorCommonResponse.Description)
                : throw new ArgumentNullException(nameof(errorCommonResponse));
    }

}
