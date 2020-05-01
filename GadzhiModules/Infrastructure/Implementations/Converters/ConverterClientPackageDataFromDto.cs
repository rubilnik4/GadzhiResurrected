using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private readonly IDialogServiceStandard _dialogServiceStandard;

        public ConverterClientPackageDataFromDto(IFileSystemOperations fileSystemOperations, IProjectSettings projectSettings,
                                                 IDialogServiceStandard dialogServiceStandard)
        {
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            _dialogServiceStandard = dialogServiceStandard ?? throw new ArgumentNullException(nameof(dialogServiceStandard));
        }

        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public PackageStatus ToPackageStatusFromIntermediateResponse(PackageDataIntermediateResponseClient packageDataIntermediateResponse)
        {
            var packageStatus = packageDataIntermediateResponse?.FileDatas?.Select(ToFileStatusFromIntermediateResponse);
            var queueStatus = ConvertToQueueInfoFromResponse(packageDataIntermediateResponse?.FilesQueueInfo);

            return new PackageStatus(packageStatus,
                                      packageDataIntermediateResponse?.StatusProcessingProject ?? StatusProcessingProject.Error,
                                      queueStatus);
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        public PackageStatus ToPackageStatus(PackageDataResponseClient packageDataResponse)
        {
            var fileDataStatusResponse = ToPackageStatusFromResponse(packageDataResponse);
            return new PackageStatus(fileDataStatusResponse, StatusProcessingProject.Writing);
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        public async Task<PackageStatus> ToFilesStatusAndSaveFiles(PackageDataResponseClient packageDataResponse)
        {
            var fileDataStatusResponse = await ToPackageStatusFromResponseAndSaveFiles(packageDataResponse);
            var filesStatus = new PackageStatus(fileDataStatusResponse, StatusProcessingProject.End);
            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private IEnumerable<FileStatus> ToPackageStatusFromResponse(PackageDataResponseClient packageDataResponse) =>
            packageDataResponse?.FilesData?.Select(ConvertToFileStatusFromResponse);

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<IEnumerable<FileStatus>> ToPackageStatusFromResponseAndSaveFiles(PackageDataResponseClient packageDataResponse)
        {
            var filesStatusTask = packageDataResponse?.FilesData?.Select(ToFileStatusFromResponseAndSaveFile);
            var filesStatus = await Task.WhenAll(filesStatusTask ?? Enumerable.Empty<Task<FileStatus>>());
            return filesStatus;
        }

        /// <summary>
        /// Конвертер информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        private static FileStatus ToFileStatusFromIntermediateResponse(FileDataIntermediateResponseClient fileIntermediateResponse) =>
            new FileStatus(fileIntermediateResponse.FilePath,
                           fileIntermediateResponse.StatusProcessing,
                           fileIntermediateResponse.FileConvertErrorTypes);

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private static FileStatus ConvertToFileStatusFromResponse(FileDataResponseClient fileResponse) =>
             new FileStatus(fileResponse.FilePath, StatusProcessing.Writing, fileResponse.FileConvertErrorTypes);

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<FileStatus> ToFileStatusFromResponseAndSaveFile(FileDataResponseClient fileResponse)
        {
            var fileConvertSavedErrorType = await SaveFileDataSourceFromDtoResponse(fileResponse);
            var fileConvertErrorTypes = fileResponse.FileConvertErrorTypes.
                                        UnionNotNull(fileConvertSavedErrorType).
                                        Where(error => error != FileConvertErrorType.NoError);

            return new FileStatus(fileResponse.FilePath, StatusProcessing.End, fileConvertErrorTypes);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<IEnumerable<FileConvertErrorType>> SaveFileDataSourceFromDtoResponse(FileDataResponseClient fileDataResponse)
        {
            if (fileDataResponse.FileDatasSourceResponseClient == null)
                return new List<FileConvertErrorType>() { FileConvertErrorType.IncorrectDataSource };

            string fileDirectoryName = Path.GetDirectoryName(fileDataResponse.FilePath);
            string convertingDirectoryName = Path.Combine(fileDirectoryName ?? throw new InvalidOperationException(nameof(fileDirectoryName)),
                                                          _projectSettings.DirectoryForSavingConvertedFiles);

            var fileConvertErrorTypeTasks = fileDataResponse.FileDatasSourceResponseClient?.
                                            Select(fileData => SaveFileDataSourceFromDtoResponse(fileData, convertingDirectoryName));
            return await Task.WhenAll(fileConvertErrorTypeTasks);
        }

        /// <summary>
        /// Сохранить файл из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileConvertErrorType> SaveFileDataSourceFromDtoResponse(FileDataSourceResponseClient fileDataSourceResponseClient,
                                                                                   string convertingDirectoryName)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileDataSourceResponseClient.FileName);
            string fileExtension = FileSystemOperations.ExtensionWithoutPoint(Path.GetExtension(fileDataSourceResponseClient.FileName));
            string directoryPath = _fileSystemOperations.CreateFolderByName(convertingDirectoryName, fileExtension.ToUpper(CultureInfo.CurrentCulture));

            if (String.IsNullOrWhiteSpace(directoryPath)) return FileConvertErrorType.RejectToSave;
            if (fileDataSourceResponseClient.FileDataSource?.Count == 0) return FileConvertErrorType.FileNotFound;

            string filePath = FileSystemOperations.CombineFilePath(directoryPath, fileName, fileExtension);
            Task<bool> UnzipFileAndSaveBool() => _fileSystemOperations.UnzipFileAndSave(filePath, fileDataSourceResponseClient.FileDataSource);

            await _dialogServiceStandard.
                  RetryOrIgnoreBoolFunction(UnzipFileAndSaveBool, $"Файл {filePath} открыт или используется. Повторить попытку сохранения?");

            return FileConvertErrorType.NoError;
        }
        /// <summary>
        /// Конвертер из трансферной модели информации в клиентскую
        /// </summary>       
        private static QueueStatus ConvertToQueueInfoFromResponse(FilesQueueInfoResponseClient filesQueueInfoResponse) =>
            new QueueStatus(filesQueueInfoResponse.FilesInQueueCount, filesQueueInfoResponse.PackagesInQueueCount);
    }

}
