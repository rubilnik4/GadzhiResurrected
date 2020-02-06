using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private IFileSystemOperations FileSystemOperations { get; }

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private IProjectSettings ProjectSettings { get; }

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>        
        private IDialogServiceStandard DialogServiceStandard { get; }

        public ConverterClientFilesDataFromDTO(IFileSystemOperations fileSystemOperations,
                                               IProjectSettings projectSettings,
                                               IDialogServiceStandard dialogServiceStandard)
        {
            FileSystemOperations = fileSystemOperations;
            ProjectSettings = projectSettings;
            DialogServiceStandard = dialogServiceStandard;
        }

        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public FilesStatus ConvertToFilesStatusFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            var filesDataStatus = filesDataIntermediateResponse?.
                                  FilesData?.
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
        public FilesStatus ConvertToFilesStatus(FilesDataResponse filesDataResponse)
        {
            var fileDataStatusResponse = ConvertToFilesStatusFromResponse(filesDataResponse);
            var filesStatus = new FilesStatus(fileDataStatusResponse,
                                              StatusProcessingProject.Writing);

            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        public async Task<FilesStatus> ConvertToFilesStatusAndSaveFiles(FilesDataResponse filesDataResponse)
        {
            var fileDataStatusResponse = await ConvertToFilesStatusFromResponseAndSaveFiles(filesDataResponse);
            var filesStatus = new FilesStatus(fileDataStatusResponse,
                                              StatusProcessingProject.End);
            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private IEnumerable<FileStatus> ConvertToFilesStatusFromResponse(FilesDataResponse filesDataResponse)
        {
            var filesStatus = filesDataResponse?.
                                  FilesData?.
                                  Select(fileResponse => ConvertToFileStatusFromResponse(fileResponse));
            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<IEnumerable<FileStatus>> ConvertToFilesStatusFromResponseAndSaveFiles(FilesDataResponse filesDataResponse)
        {
            var filesStatusTask = filesDataResponse?.
                                  FilesData?.
                                  Select(fileResponse => ConvertToFileStatusFromResponseAndSaveFile(fileResponse));
            var filesStatus = await Task.WhenAll(filesStatusTask);
            return filesStatus;
        }

        /// <summary>
        /// Конвертер информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        private FileStatus ConvertToFileStatusFromIntermediateResponse(FileDataIntermediateResponse fileIntermediateResponse)
        {
            return new FileStatus(fileIntermediateResponse.FilePath,
                                  fileIntermediateResponse.StatusProcessing,
                                  fileIntermediateResponse.FileConvertErrorType);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private FileStatus ConvertToFileStatusFromResponse(FileDataResponse fileResponse)
        {
            StatusProcessing statusProcessing = fileResponse.StatusProcessing == StatusProcessing.Completed ?
                                                StatusProcessing.Writing :
                                                StatusProcessing.Error;

            return new FileStatus(fileResponse.FilePath,
                                  statusProcessing,
                                  fileResponse.FileConvertErrorType);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private async Task<FileStatus> ConvertToFileStatusFromResponseAndSaveFile(FileDataResponse fileResponse)
        {
            FileSavedCheck fileSavedCheck = await SaveFileFromDTOResponse(fileResponse);

            StatusProcessing statusProcessing = fileResponse.StatusProcessing == StatusProcessing.Completed &&
                                                fileSavedCheck.IsSaved ?
                                                StatusProcessing.End :
                                                StatusProcessing.Error;

            var fileConvertErrorTypes = fileResponse.FileConvertErrorType?.Union(fileSavedCheck.Errors);

            return new FileStatus(fileResponse.FilePath,
                                  statusProcessing,
                                  fileConvertErrorTypes);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileSavedCheck> SaveFileFromDTOResponse(FileDataResponse fileDataResponse)
        {
            var fileSavedCheck = new FileSavedCheck();

            bool isValidDataSource = fileDataResponse.FileDataSource != null;
            if (isValidDataSource)
            {
                string createdDirectoryName = Path.GetDirectoryName(fileDataResponse.FilePath);
                string fileName = Path.GetFileNameWithoutExtension(fileDataResponse.FilePath);
                string fileExtension = FileHelpers.ExtensionWithoutPoint(Path.GetExtension(fileDataResponse.FilePath));

                (bool isCreated, string directoryPath) = FileSystemOperations.
                                                         CreateFolderByName(createdDirectoryName,
                                                                            ProjectSettings.DirectoryForSavingConvertedFiles);
                if (isCreated)
                {
                    fileSavedCheck.FilePath = FileSystemOperations.CombineFilePath(directoryPath, fileName, fileExtension);
                    await DialogServiceStandard.RetryOrIgnoreBoolFunction(async () =>
                            fileSavedCheck.IsSaved = await FileSystemOperations.
                                                     UnzipFileAndSave(fileSavedCheck.FilePath, fileDataResponse.FileDataSource),
                                                                      $"Файл {fileSavedCheck.FilePath} открыт или используется. Повторить попытку сохранения?");
                }
                else
                {
                    fileSavedCheck.AddError(FileConvertErrorType.RejectToSave);
                }
            }
            else
            {
                fileSavedCheck.AddError(FileConvertErrorType.IncorrectDataSource);
            }

            return fileSavedCheck;
        }

        /// <summary>
        /// Конвертер из трансферной модели информации в клиентскую
        /// </summary>       
        private FilesQueueStatus ConvertToFilesQueueInfoFromResponse(FilesQueueInfoResponse filesQueueInfoResponse)
        {
            return new FilesQueueStatus(filesQueueInfoResponse.FilesInQueueCount,
                                      filesQueueInfoResponse.PackagesInQueueCount);
        }
    }

}
