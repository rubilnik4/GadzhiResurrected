using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FilesDataFromDTOConverterToClient
    {
        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public static FilesStatus ConvertToFilesStatusFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            var filesDataStatus = filesDataIntermediateResponse?.
                                  FilesData?.
                                  Select(fileResponse => ConvertToFileStatusFromIntermediateResponse(fileResponse));

            FilesQueueInfo filesQueueInfo = ConvertToFilesQueueInfoFromResponse(filesDataIntermediateResponse?.
                                                                                FilesQueueInfo);

            var filesStatusIntermediate = new FilesStatus(filesDataStatus,
                                                          filesDataIntermediateResponse.StatusProcessingProject,
                                                          filesQueueInfo);
            return filesStatusIntermediate;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        public static FilesStatus ConvertToFilesStatus(FilesDataResponse filesDataResponse)
        {
            var fileDataStatusResponse = ConvertToFilesStatusFromResponse(filesDataResponse);
            var filesStatus = new FilesStatus(fileDataStatusResponse,
                                              StatusProcessingProject.Writing);

            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        public static async Task<FilesStatus> ConvertToFilesStatusAndSaveFiles(FilesDataResponse filesDataResponse,
                                                                               IFileSystemOperations fileSystemOperations)
        {
            var fileDataStatusResponse = await ConvertToFilesStatusFromResponseAndSaveFiles(filesDataResponse, fileSystemOperations);
            var filesStatus = new FilesStatus(fileDataStatusResponse,
                                              StatusProcessingProject.End);

            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private static IEnumerable<FileStatus> ConvertToFilesStatusFromResponse(FilesDataResponse filesDataResponse)
        {
            var filesStatus = filesDataResponse?.
                                  FilesData?.
                                  Select(fileResponse => ConvertToFileStatusFromResponse(fileResponse));           
            return filesStatus;
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private static async Task<IEnumerable<FileStatus>> ConvertToFilesStatusFromResponseAndSaveFiles(FilesDataResponse filesDataResponse,
                                                                                                        IFileSystemOperations fileSystemOperations)
        {
            var filesStatusTask = filesDataResponse?.
                   FilesData?.
                   Select(fileResponse => ConvertToFileStatusFromResponseAndSaveFile(fileResponse,
                                                                                     fileSystemOperations));
            var filesStatus = await Task.WhenAll(filesStatusTask);
            return filesStatus;
        }

        /// <summary>
        /// Конвертер информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        private static FileStatus ConvertToFileStatusFromIntermediateResponse(FileDataIntermediateResponse fileIntermediateResponse)
        {
            return new FileStatus(fileIntermediateResponse.FilePath,
                                  fileIntermediateResponse.StatusProcessing,
                                  fileIntermediateResponse.FileConvertErrorType);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части перед сохранением
        /// </summary>      
        private static FileStatus ConvertToFileStatusFromResponse(FileDataResponse fileResponse)
        {           
            StatusProcessing statusProcessing = fileResponse.StatusProcessing == StatusProcessing.Completed?                                             
                                                StatusProcessing.Writing :
                                                StatusProcessing.Error;
            
            return new FileStatus(fileResponse.FilePath,
                                  statusProcessing,
                                  fileResponse.FileConvertErrorType);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части и сохранение файла
        /// </summary>      
        private static async Task<FileStatus> ConvertToFileStatusFromResponseAndSaveFile(FileDataResponse fileResponse,
                                                                                         IFileSystemOperations fileSystemOperations)
        {
            FileSavedCheck fileSavedCheck = await SaveFileFromDTOResponse(fileResponse, fileSystemOperations);

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
        private static async Task<FileSavedCheck> SaveFileFromDTOResponse(FileDataResponse fileDataResponse, IFileSystemOperations fileSystemOperations)
        {
            var fileSavedCheck = new FileSavedCheck();

            bool isValidDataSource = fileDataResponse.FileDataSource != null;
            if (isValidDataSource)
            {
                string createdDirectoryName = Path.GetDirectoryName(fileDataResponse.FilePath);
                string fileName = Path.GetFileNameWithoutExtension(fileDataResponse.FilePath);
                string fileExtension = FileHelpers.ExtensionWithoutPoint(Path.GetExtension(fileDataResponse.FilePath));

                (bool isCreated, string directoryPath) = fileSystemOperations.CreateFolderByName(createdDirectoryName, "Converted");
                if (isCreated)
                {
                    fileSavedCheck.FilePath = fileSystemOperations.CombineFilePath(directoryPath, fileName, fileExtension);
                    await fileSystemOperations.UnzipFileAndSave(fileSavedCheck.FilePath, fileDataResponse.FileDataSource);

                    fileSavedCheck.IsSaved = true;
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
        private static FilesQueueInfo ConvertToFilesQueueInfoFromResponse(FilesQueueInfoResponse filesQueueInfoResponse)
        {
            return new FilesQueueInfo(filesQueueInfoResponse.FilesInQueueCount,
                                      filesQueueInfoResponse.PackagesInQueueCount);
        }
    }

}
