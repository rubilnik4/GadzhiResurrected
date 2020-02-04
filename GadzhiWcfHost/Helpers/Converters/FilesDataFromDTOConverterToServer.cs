using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Helpers.Converters
{
    public static class FilesDataFromDTOConverterToServer
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        public static async Task<FilesDataServer> ConvertToFilesDataServerAndSaveFile(FilesDataRequest filesDataRequest, IFileSystemOperations fileSystemOperations)
        {
            var filesDataServerToConvertTask = filesDataRequest?.FilesData?.Select(fileDTO =>
                                               ConvertToFileDataServerAndSaveFile(fileDTO, 
                                                                                  filesDataRequest.ID.ToString(), 
                                                                                  fileSystemOperations));
            var filesDataServerToConvert = await Task.WhenAll(filesDataServerToConvertTask);

            return new FilesDataServer(filesDataRequest.ID, filesDataServerToConvert);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс
        /// </summary>      
        private static async Task<FileDataServer> ConvertToFileDataServerAndSaveFile(FileDataRequest fileDataRequest,
                                                                                     string packageGuid,
                                                                                     IFileSystemOperations fileSystemOperations)
        {
            FileSavedCheck fileSavedCheck = await SaveFileFromDTORequest(fileDataRequest, packageGuid, fileSystemOperations);

            return new FileDataServer(fileSavedCheck.FilePath,
                                      fileDataRequest.FilePath,
                                      fileDataRequest.ColorPrint,
                                      fileSavedCheck.Errors);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private static async Task<FileSavedCheck> SaveFileFromDTORequest(FileDataRequest fileDataRequest, 
                                                                         string packageGuid, 
                                                                         IFileSystemOperations fileSystemOperations)
        {
            var fileSavedCheck = new FileSavedCheck();

            var (isValid, errorsFromValidation) = ValidateDTOData.IsFileDataRequestValid(fileDataRequest);
            if (isValid)
            {
                (bool isCreated, string directoryPath) = fileSystemOperations.CreateFolderByName(FileSystemInformation.ConvertionDirectory, packageGuid);
                if (isCreated)
                {
                    fileSavedCheck.FilePath = fileSystemOperations.CombineFilePath(directoryPath, Guid.NewGuid().ToString(), fileDataRequest.FileExtension);
                    await fileSystemOperations.UnzipFileAndSave(fileSavedCheck.FilePath, fileDataRequest.FileDataSource);

                    fileSavedCheck.IsSaved = true;
                }
                else
                {
                    fileSavedCheck.AddError(FileConvertErrorType.RejectToSave);
                }
            }
            else
            {
                if (errorsFromValidation != null)
                {
                    fileSavedCheck.AddErrors(errorsFromValidation);
                }
            }

            return fileSavedCheck;
        }
    }
}
