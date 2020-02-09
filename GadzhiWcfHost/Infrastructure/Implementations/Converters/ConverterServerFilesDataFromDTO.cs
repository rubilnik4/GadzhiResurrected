using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Helpers;
using GadzhiWcfHost.Infrastructure.Interfaces.Converters;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в серверную
    /// </summary>      
    public class ConverterServerFilesDataFromDTO: IConverterServerFilesDataFromDTO
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConverterServerFilesDataFromDTO(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        public async Task<FilesDataServer> ConvertToFilesDataServerAndSaveFile(FilesDataRequest filesDataRequest)
        {
            var filesDataServerToConvertTask = filesDataRequest?.FilesData?.Select(fileDTO =>
                                               ConvertToFileDataServerAndSaveFile(fileDTO, 
                                                                                  filesDataRequest.ID.ToString()));
            var filesDataServerToConvert = await Task.WhenAll(filesDataServerToConvertTask);

            return new FilesDataServer(filesDataRequest.ID, filesDataServerToConvert);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс
        /// </summary>      
        private async Task<FileDataServer> ConvertToFileDataServerAndSaveFile(FileDataRequest fileDataRequest,
                                                                              string packageGuid)
        {
            FileSavedCheck fileSavedCheck = await SaveFileFromDTORequest(fileDataRequest, packageGuid);

            return new FileDataServer(fileSavedCheck.FilePath,
                                      fileDataRequest.FilePath,
                                      fileDataRequest.ColorPrint,
                                      fileSavedCheck.Errors);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileSavedCheck> SaveFileFromDTORequest(FileDataRequest fileDataRequest, 
                                                                  string packageGuid)
        {
            var fileSavedCheck = new FileSavedCheck();

            var (isValid, errorsFromValidation) = ValidateDTOData.IsFileDataRequestValid(fileDataRequest);
            if (isValid)
            {
                (bool isCreated, string directoryPath) = _fileSystemOperations.CreateFolderByName(FileSystemInformation.ConvertionDirectory, packageGuid);
                if (isCreated)
                {
                    fileSavedCheck.FilePath = _fileSystemOperations.CombineFilePath(directoryPath, 
                                                                                   Guid.NewGuid().ToString(),
                                                                                   FileHelpers.ExtensionWithoutPointFromPath(fileDataRequest.FilePath));
                    fileSavedCheck.IsSaved = await _fileSystemOperations.UnzipFileAndSave(fileSavedCheck.FilePath, fileDataRequest.FileDataSource);
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
