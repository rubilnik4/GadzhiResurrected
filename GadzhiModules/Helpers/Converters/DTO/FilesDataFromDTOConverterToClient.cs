using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Information;
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
        public static FilesStatus ConvertToFilesStatusFromIntermediateResponse(FilesDataIntermediateResponse filesDataIntermediateResponse)
        {
            var filesDataStatus = filesDataIntermediateResponse?.
                                  FilesData?.
                                  Select(fileResponse => ConvertToFileStatusFromIntermediateResponse(fileResponse));

            FilesQueueInfo filesQueueInfo = ConvertToFilesQueueInfoFromResponse(filesDataIntermediateResponse?.
                                                                                FilesQueueInfo);
           
            var filesStatusIntermediate = new FilesStatus(filesDataStatus,
                                                          filesDataIntermediateResponse.StatusProcessingProject,
                                                          filesQueueInfo);
            return filesStatusIntermediate;
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

        private static FilesQueueInfo ConvertToFilesQueueInfoFromResponse(FilesQueueInfoResponse filesQueueInfoResponse)
        {
            return new FilesQueueInfo(filesQueueInfoResponse.FilesInQueueCount,
                                      filesQueueInfoResponse.PackagesInQueueCount);
        }
    }
}
