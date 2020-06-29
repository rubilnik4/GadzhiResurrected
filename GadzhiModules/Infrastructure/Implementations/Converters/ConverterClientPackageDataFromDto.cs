using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;

namespace GadzhiModules.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертеры из трансферной модели в локальную
    /// </summary>  
    public class ConverterClientPackageDataFromDto : IConverterClientPackageDataFromDto
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private readonly IDialogService _dialogService;

        public ConverterClientPackageDataFromDto(IFileSystemOperations fileSystemOperations, IDialogService dialogService)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public PackageStatus ToPackageStatusFromIntermediateResponse(PackageDataIntermediateResponseClient packageDataIntermediateResponse)
        {
            if (packageDataIntermediateResponse == null) throw new ArgumentNullException(nameof(packageDataIntermediateResponse));

            var packageStatus = packageDataIntermediateResponse.FilesData?.Select(ToFileStatusFromIntermediateResponse);
            var queueStatus = ConvertToQueueInfoFromResponse(packageDataIntermediateResponse.FilesQueueInfo);

            return new PackageStatus(packageStatus,
                                      packageDataIntermediateResponse.StatusProcessingProject,
                                      queueStatus);
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        public PackageStatus ToPackageStatus(PackageDataResponseClient packageDataResponse)
        {
            if (packageDataResponse == null) throw new ArgumentNullException(nameof(packageDataResponse));

            var fileDataStatusResponse = ToPackageStatusFromResponse(packageDataResponse);
            return new PackageStatus(fileDataStatusResponse, StatusProcessingProject.Writing);
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        public async Task<PackageStatus> ToFilesStatusAndSaveFiles(PackageDataResponseClient packageDataResponse)
        {
            if (packageDataResponse == null) throw new ArgumentNullException(nameof(packageDataResponse));

            var fileDataStatusResponse = await ToPackageStatusFromResponseAndSaveFiles(packageDataResponse);
            var filesStatus = new PackageStatus(fileDataStatusResponse, StatusProcessingProject.End);
            return filesStatus;
        }

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
        private static FileStatus ToFileStatusFromIntermediateResponse(FileDataIntermediateResponseClient fileResponse) =>
            new FileStatus(fileResponse.FilePath,
                           fileResponse.StatusProcessing,
                           fileResponse.FileErrors.Select(ToErrorCommon));

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private static FileStatus ConvertToFileStatusFromResponse(FileDataResponseClient fileResponse) =>
             new FileStatus(fileResponse.FilePath, StatusProcessing.Writing,
                            fileResponse.FileErrors.Select(ToErrorCommon));

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<FileStatus> ToFileStatusFromResponseAndSaveFile(FileDataResponseClient fileResponse)
        {
            var fileConvertSavedErrorType = await SaveFileDataSourceFromDtoResponse(fileResponse);
            var fileConvertErrorTypes = fileResponse.FileErrors.Select(ToErrorCommon).
                                        UnionNotNull(fileConvertSavedErrorType).
                                        Where(error => error.FileConvertErrorType != FileConvertErrorType.NoError);

            return new FileStatus(fileResponse.FilePath, StatusProcessing.End, fileConvertErrorTypes);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<IEnumerable<IErrorCommon>> SaveFileDataSourceFromDtoResponse(FileDataResponseClient fileDataResponse)
        {
            if (fileDataResponse.FilesDataSource == null)
                return new List<IErrorCommon>() { new ErrorCommon(FileConvertErrorType.IncorrectDataSource, "Некорректные входные данные") };

            string fileDirectoryName = Path.GetDirectoryName(fileDataResponse.FilePath);
            string convertingDirectoryName = Path.Combine(fileDirectoryName ?? throw new InvalidOperationException(nameof(fileDirectoryName)),
                                                          ProjectSettings.DirectoryForSavingConvertedFiles);

            var fileConvertErrorTypeTasks = fileDataResponse.FilesDataSource?.
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
            string fileExtension = FileSystemOperations.ExtensionWithoutPoint(Path.GetExtension(fileDataSourceResponseClient.FileName));
            string fileExtensionValid = ValidFileExtensions.GetFileTypesValid(fileExtension).ToString().ToLowerCaseCurrentCulture();
            string directoryPath = _fileSystemOperations.CreateFolderByName(convertingDirectoryName, fileExtensionValid.ToUpperCaseCurrentCulture());


            if (String.IsNullOrWhiteSpace(fileName)) return new ErrorCommon(FileConvertErrorType.IncorrectFileName, $"Некорректное имя файла {fileName}");
            if (String.IsNullOrWhiteSpace(fileName)) return new ErrorCommon(FileConvertErrorType.IncorrectExtension, $"Некорректное расширение файла {fileExtension}");
            if (String.IsNullOrWhiteSpace(directoryPath)) return new ErrorCommon(FileConvertErrorType.RejectToSave, "Директория сохранения не создана");
            if (fileDataSourceResponseClient.FileDataSource.Length == 0) return new ErrorCommon(FileConvertErrorType.IncorrectDataSource,
                                                                                                $"Некорректные входные данные {fileName}");

            string filePath = FileSystemOperations.CombineFilePath(directoryPath, fileName, fileExtensionValid);
            Task<bool> UnzipFileAndSaveBool() => _fileSystemOperations.UnzipFileAndSave(filePath, fileDataSourceResponseClient.FileDataSource);

            await _dialogService.RetryOrIgnoreBoolFunction(UnzipFileAndSaveBool,
                                                                   $"Файл {filePath} открыт или используется. Повторить попытку сохранения?");

            return new ErrorCommon(FileConvertErrorType.NoError, "Ошибки отсутствуют");
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
                ? new ErrorCommon(errorCommonResponse.FileConvertErrorType, errorCommonResponse.ErrorDescription)
                : throw new ArgumentNullException(nameof(errorCommonResponse));
    }

}
