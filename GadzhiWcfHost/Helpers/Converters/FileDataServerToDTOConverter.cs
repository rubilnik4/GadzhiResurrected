using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Helpers.Converters
{
    public static class FileDataServerToDTOConverter
    {
        public static FileDataIntermediateResponse ConvertToIntermediateResponse(FileDataServer fileDataServer)
        {
            return new FileDataIntermediateResponse()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.IsValid ?
                                    StatusProcessing.InQueue :
                                    StatusProcessing.Error,

                FileConvertErrorType = fileDataServer.FileConvertErrorType,
            };
        }
    }
}