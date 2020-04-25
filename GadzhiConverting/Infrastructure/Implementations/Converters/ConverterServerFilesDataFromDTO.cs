using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.TransferModels.FilesConvert.Base;
using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в серверную
    /// </summary>      
    public class ConverterServerFilesDataFromDTO : IConverterServerFilesDataFromDTO
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        public ConverterServerFilesDataFromDTO(IFileSystemOperations fileSystemOperations,
                                               IProjectSettings projectSettings)
        {
            _fileSystemOperations = fileSystemOperations;
            _projectSettings = projectSettings;
        }

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        public async Task<IFilesDataServer> ConvertToFilesDataServerAndSaveFile(FilesDataRequestServer filesDataRequest)
        {
            var filesDataServerToConvertTask = filesDataRequest?.FileDatas?.Select(fileDTO =>
                                               ConvertToFileDataServerAndSaveFile(fileDTO, filesDataRequest.Id.ToString()));
            var filesDataServerToConvert = await Task.WhenAll(filesDataServerToConvertTask);

            return new FilesDataServer(filesDataRequest.Id, filesDataRequest.AttemptingConvertCount, 
                                       StatusProcessingProject.Converting, filesDataServerToConvert);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс
        /// </summary>      
        private async Task<IFileDataServer> ConvertToFileDataServerAndSaveFile(FileDataRequestServer fileDataRequest, string packageGuid)
        {
            FileSavedCheck fileSavedCheck = await SaveFileFromDTORequest(fileDataRequest, packageGuid);

            return new FileDataServer(fileSavedCheck.FilePath, fileDataRequest.FilePath, fileDataRequest.ColorPrint,
                                      StatusProcessing.InQueue, fileSavedCheck.Errors);
        }

        /// <summary>
        /// Сохранить данные из трансферной модели на жесткий диск
        /// </summary>      
        private async Task<FileSavedCheck> SaveFileFromDTORequest(FileDataRequestServer fileDataRequest, string packageGuid)
        {
            var fileSavedCheck = new FileSavedCheck();

            var (isValid, errorsFromValidation) = ValidateDTOData.IsFileDataRequestValid((FileDataRequestBase)fileDataRequest);
            if (isValid)
            {
                string directoryPath = _fileSystemOperations.CreateFolderByName(_projectSettings.ConvertingDirectory, packageGuid);
                if (!String.IsNullOrWhiteSpace(directoryPath))
                {
                    fileSavedCheck.FilePath = Path.Combine(directoryPath, Path.GetFileName(fileDataRequest.FilePath));
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
