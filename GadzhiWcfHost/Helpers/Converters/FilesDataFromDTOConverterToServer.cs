using GadzhiCommon.Enums.FilesConvert;
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
        /// Конвертер информации из трансферной модели в класс серверной части
        /// </summary>      
        public static async Task<FileDataServer> ConvertToFileDataServerAndSaveFile(FileDataRequest fileDataRequest, IFileSystemOperations fileSystemOperations)
        {
            FileSavedCheck fileSavedCheck = await SaveFileFromDTORequest(fileDataRequest, fileSystemOperations);

            return new FileDataServer(fileSavedCheck.FilePath, 
                                      fileDataRequest.FilePath, 
                                      fileDataRequest.ColorPrint, 
                                      fileSavedCheck.Errors);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private static async Task<FileSavedCheck> SaveFileFromDTORequest(FileDataRequest fileDataRequest, IFileSystemOperations fileSystemOperations)
        {
            var fileSavedCheck = new FileSavedCheck();

            var (isValid, errorsFromValidation) = ValidateDTOData.IsFileDataRequestValid(fileDataRequest);
            if (isValid)
            {
                (bool isCreated, string directoryPath) = fileSystemOperations.CreateFolderByGuid(FileSystemInformation.ApplicationPath);
                if (isCreated)
                {
                    fileSavedCheck.FilePath = fileSystemOperations.CreateFilePath(directoryPath, Guid.NewGuid().ToString(), fileDataRequest.FileExtension);
                    await fileSystemOperations.UnzipFileAndSave(fileSavedCheck.FilePath, fileDataRequest.FileDataSource);

                    fileSavedCheck.IsSaved = true;
                }
                else
                {
                    fileSavedCheck.Errors.Add(FileConvertErrorType.RejectToSave);
                }
            }
            else
            {
                if (errorsFromValidation != null)
                {
                    fileSavedCheck.Errors.AddRange(errorsFromValidation);
                }
            }

            return fileSavedCheck;
        }
    }
}
