using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FilesDataClientToDTOConverter
    {
        /// <summary>
        /// Конвертер пакета информации о файле из локальной модели в трансферную
        /// </summary>      
        public static async Task<FilesDataRequest> ConvertToFilesDataRequest(IFilesData filesData, IFileSystemOperations fileSystemOperations)
        {
            var filesRequestExist = await Task.WhenAll(filesData.FilesInfo?.Where(file => fileSystemOperations.IsFileExist(file.FilePath))?.
                                                                                    Select(file => ConvertToFileDataRequest(file, fileSystemOperations)));
            var filesRequestEnsuredWithBytes = filesRequestExist?.Where(file => file.FileDataSource != null);

            return new FilesDataRequest()
            {
                ID = filesData.ID,
                FilesData = filesRequestEnsuredWithBytes,
            };
        }

        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        private static async Task<FileDataRequest> ConvertToFileDataRequest(FileData fileData, IFileSystemOperations fileSystemOperations)
        {
            byte[] fileDataSource = await fileSystemOperations.ConvertFileToByteAndZip(fileData.FileName, fileData.FilePath);

            return new FileDataRequest()
            {
                ColorPrint = fileData.ColorPrint,
                FileName = fileData.FileName,
                FilePath = fileData.FilePath,
                FileExtension = fileData.FileExtension,
                StatusProcessing = fileData.StatusProcessing,
                FileDataSource = fileDataSource,
            };
        }
    }
}
