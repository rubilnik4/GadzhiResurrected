using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Helpers.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в промежуточную транспортную
    /// </summary>
    public static class FilesDataServerToDTOConverter
    {

        public static FilesDataIntermediateResponse ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer)
        {
            return new FilesDataIntermediateResponse()
            {
                FilesData = filesDataServer?.FilesDataInfo?.Select(fileDataServer =>
                                             FilesDataServerToDTOConverter.ConvertFileToIntermediateResponse(fileDataServer)),
            };
        }

        private static FileDataIntermediateResponse ConvertFileToIntermediateResponse(FileDataServer fileDataServer)
        {
            return new FileDataIntermediateResponse()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileConvertErrorType = fileDataServer.FileConvertErrorType,
            };
        }
    }
}