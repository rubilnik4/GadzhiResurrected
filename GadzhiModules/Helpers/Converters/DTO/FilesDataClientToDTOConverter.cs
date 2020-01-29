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
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        public static async Task<FileDataRequest> ConvertToFileDataDTO(FileData fileData, IFileSystemOperations fileSystemOperations)
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
