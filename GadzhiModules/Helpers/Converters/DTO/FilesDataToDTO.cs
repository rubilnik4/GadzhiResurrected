using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FilesDataToDTOConverter
    {
        /// <summary>
        /// Конвертер информации о файлах из локальной модели в трансферную
        /// </summary>      
        public static IEnumerable<FileDataRequest> ConvertToFilesDataDTO(IEnumerable<FileData> fileData)
        {
            return fileData?.Select(file => FileDataToDTOConverter.ConvertToFileDataDTO(file));
        }
    }
}
