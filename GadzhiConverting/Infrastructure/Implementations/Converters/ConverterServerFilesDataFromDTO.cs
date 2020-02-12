﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiDAL.Infrastructure.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
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
        public async Task<FilesDataServer> ConvertToFilesDataServerAndSaveFile(FilesDataRequest filesDataRequest)
        {
            var filesDataServerToConvertTask = filesDataRequest?.FilesData?.Select(fileDTO =>
                                               ConvertToFileDataServerAndSaveFile(fileDTO, 
                                                                                  filesDataRequest.Id.ToString()));
            var filesDataServerToConvert = await Task.WhenAll(filesDataServerToConvertTask);

            return new FilesDataServer(filesDataRequest.Id, filesDataServerToConvert);
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
                (bool isCreated, string directoryPath) = _fileSystemOperations.CreateFolderByName(_projectSettings.ConvertingDirectory, 
                                                                                                  packageGuid);
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