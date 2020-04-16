using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Helpers.FileSystem;
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
    public class ConverterClientFilesDataFromDTO : IConverterClientFilesDataFromDTO
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

        public ConverterClientFilesDataFromDTO(IFileSystemOperations fileSystemOperations,
                                               IProjectSettings projectSettings,
                                               IDialogServiceStandard dialogServiceStandard)
        {
            _fileSystemOperations = fileSystemOperations;
            _projectSettings = projectSettings;
            _dialogServiceStandard = dialogServiceStandard;
        }

        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public FilesStatus ConvertToFilesStatusFromIntermediateResponse(FilesDataIntermediateResponseClient filesDataIntermediateResponse)
        {
            var filesDataStatus = filesDataIntermediateResponse?.
                                  FileDatas?.
                                  Select(fileResponse => ConvertToFileStatusFromIntermediateResponse(fileResponse));

            FilesQueueStatus filesQueueStatus = ConvertToFilesQueueInfoFromResponse(filesDataIntermediateResponse?.
                                                                                FilesQueueInfo);

            var filesStatusIntermediate = new FilesStatus(filesDataStatus,
                                                          filesDataIntermediateResponse.StatusProcessingProject,
                                                          filesQueueStatus);
            return filesStatusIntermediate;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        public FilesStatus ConvertToFilesStatus(FilesDataResponseClient filesDataResponse)
        {
            var fileDataStatusResponse = ConvertToFilesStatusFromResponse(filesDataResponse);
            var filesStatus = new FilesStatus(fileDataStatusResponse,
                                              StatusProcessingProject.Writing);

            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        public async Task<FilesStatus> ConvertToFilesStatusAndSaveFiles(FilesDataResponseClient filesDataResponse)
        {
            var fileDataStatusResponse = await ConvertToFilesStatusFromResponseAndSaveFiles(filesDataResponse);
            var filesStatus = new FilesStatus(fileDataStatusResponse,
                                              StatusProcessingProject.End);
            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private IEnumerable<FileStatus> ConvertToFilesStatusFromResponse(FilesDataResponseClient filesDataResponse)
        {
            var filesStatus = filesDataResponse?.
                                  FileDatas?.
                                  Select(fileResponse => ConvertToFileStatusFromResponse(fileResponse));
            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<IEnumerable<FileStatus>> ConvertToFilesStatusFromResponseAndSaveFiles(FilesDataResponseClient filesDataResponse)
        {
            var filesStatusTask = filesDataResponse?.
                                  FileDatas?.
                                  Select(fileResponse => ConvertToFileStatusFromResponseAndSaveFile(fileResponse));
            var filesStatus = await Task.WhenAll(filesStatusTask);
            return filesStatus;
        }

        /// <summary>
        /// Конвертер информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        private FileStatus ConvertToFileStatusFromIntermediateResponse(FileDataIntermediateResponseClient fileIntermediateResponse)
        {
            return new FileStatus(fileIntermediateResponse.FilePath,
                                  fileIntermediateResponse.StatusProcessing,
                                  fileIntermediateResponse.FileConvertErrorType);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private FileStatus ConvertToFileStatusFromResponse(FileDataResponseClient fileResponse) =>
             new FileStatus(fileResponse.FilePath, StatusProcessing.Writing, fileResponse.FileConvertErrorType);


        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<FileStatus> ConvertToFileStatusFromResponseAndSaveFile(FileDataResponseClient fileResponse)
        {
            var fileConvertSavedErrorType = await SaveFilesDataSourceFromDTOResponse(fileResponse);
            var fileConvertErrorTypes = fileResponse.FileConvertErrorType.UnionNotNull(fileConvertSavedErrorType).
                                                                          Where(error => error != FileConvertErrorType.NoError);

            return new FileStatus(fileResponse.FilePath, StatusProcessing.End, fileConvertErrorTypes);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<IEnumerable<FileConvertErrorType>> SaveFilesDataSourceFromDTOResponse(FileDataResponseClient fileDataResponse)
        {
            if (fileDataResponse.FileDatasSourceResponseClient != null)
            {
                string fileDirectoryName = Path.GetDirectoryName(fileDataResponse.FilePath);
                string convertingDirectoryName = Path.Combine(fileDirectoryName, _projectSettings.DirectoryForSavingConvertedFiles);

                var fileConvertErrorTypeTasks = fileDataResponse.FileDatasSourceResponseClient?.
                                                Select(fileData => SaveFileDataSourceFromDTOResponse(fileData, convertingDirectoryName));
                return await Task.WhenAll(fileConvertErrorTypeTasks);
            }
            else
            {
                return new List<FileConvertErrorType>() { FileConvertErrorType.IncorrectDataSource };
            }
        }

        /// <summary>
        /// Сохранить файл из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileConvertErrorType> SaveFileDataSourceFromDTOResponse(FileDataSourceResponseClient fileDataSourceResponseClient,
                                                                                   string convertingDirectoryName)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileDataSourceResponseClient.FileName);
            string fileExtension = FileSystemOperations.ExtensionWithoutPoint(Path.GetExtension(fileDataSourceResponseClient.FileName));

            string directoryPath = _fileSystemOperations.CreateFolderByName(convertingDirectoryName,
                                                                                              fileExtension.ToUpper(CultureInfo.CurrentCulture));
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                if (fileDataSourceResponseClient.FileDataSource?.Count != 0)
                {
                    string filePath = FileSystemOperations.CombineFilePath(directoryPath, fileName, fileExtension);
                    await _dialogServiceStandard.RetryOrIgnoreBoolFunction(async () =>
                            await _fileSystemOperations.UnzipFileAndSave(filePath, fileDataSourceResponseClient.FileDataSource),
                                                                         $"Файл {filePath} открыт или используется. Повторить попытку сохранения?");
                }
                else
                {
                    return FileConvertErrorType.FileNotFound;
                }
            }
            else
            {
                return FileConvertErrorType.RejectToSave;
            }

            return FileConvertErrorType.NoError;
        }
        /// <summary>
        /// Конвертер из трансферной модели информации в клиентскую
        /// </summary>       
        private FilesQueueStatus ConvertToFilesQueueInfoFromResponse(FilesQueueInfoResponseClient filesQueueInfoResponse)
        {
            return new FilesQueueStatus(filesQueueInfoResponse.FilesInQueueCount,
                                      filesQueueInfoResponse.PackagesInQueueCount);
        }
    }

}
