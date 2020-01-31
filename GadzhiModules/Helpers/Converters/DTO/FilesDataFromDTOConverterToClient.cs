using GadzhiCommon.Enums.FilesConvert;
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
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        public static IEnumerable<FileStatus> ConvertToFilesStatusFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
           return filesDataIntermediateResponse?.
                  FilesData?.
                  Select(fileResponse => ConvertToFileStatusFromIntermediateResponse(fileResponse));
        }

        /// <summary>
        /// Конвертер пакета информации из основной трансферной модели в класс клиентской части
        /// </summary>      
        public static IEnumerable<FileStatus> ConvertToFilesStatusFromResponse(FilesDataResponse filesDataResponse)
        {
            return filesDataResponse?.
                   FilesData?.
                   Select(fileResponse => ConvertToFileStatusFromResponse(fileResponse));
        }

        /// <summary>
        /// Конвертер информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        private static FileStatus ConvertToFileStatusFromIntermediateResponse(FileDataIntermediateResponse fileIntermediateResponse)
        {
            return new FileStatus(fileIntermediateResponse.FilePath,
                                  fileIntermediateResponse.StatusProcessing);
        }

        /// <summary>
        /// Конвертер информации из трансферной модели в класс клиентской части
        /// </summary>      
        private static FileStatus ConvertToFileStatusFromResponse(FileDataResponse fileResponse)
        {
            return new FileStatus(fileResponse.FilePath,
                                  StatusProcessing.Wrighted);
        }
    }
}
