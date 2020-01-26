using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FilesDataToDTOConverter
    {  
        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        public static async Task<FileDataRequest> ConvertToFileDataDTO(FileData fileData, IFileSeach fileSeach)
        {
            byte[] fileDataSource =await fileSeach.ConvertFileToByteAndZip(fileData.FileName, fileData.FilePath);

            return new FileDataRequest()
            {
                ColorPrint = fileData.ColorPrint,
                FileName = fileData.FileName,
                FilePath = fileData.FilePath,
                FileType = fileData.FileType,
                StatusProcessing = fileData.StatusProcessing,
                FileDataSource = fileDataSource,
            };
        }
    }
}
