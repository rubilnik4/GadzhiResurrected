using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FilesDataFromDTOConverterToClient
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части
        /// </summary>      
        public static IEnumerable<FileStatus> ConvertToFilesStatus(FilesDataIntermediateResponse filesDataResponse)
        {
           return filesDataResponse?.
                  FilesData?.
                  Select(fileResponse => ConvertToFileStatus(fileResponse));
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части
        /// </summary>      
        private static FileStatus ConvertToFileStatus(FileDataIntermediateResponse fileResponse)
        {
            return new FileStatus(fileResponse.FilePath,
                                  fileResponse.StatusProcessing,
                                  fileResponse.FileConvertErrorType);
        }
    }
}
