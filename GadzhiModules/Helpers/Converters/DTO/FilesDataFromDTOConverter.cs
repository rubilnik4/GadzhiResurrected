﻿using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FilesDataFromDTOConverter
    {
        /// <summary>
        /// Конвертер информации из трансферной модели в класс
        /// </summary>      
        public static FileStatus ConvertToFileStatus(FileDataIntermediateResponse fileResponse)
        {
            return new FileStatus(fileResponse.FilePath,
                                  fileResponse.StatusProcessing,
                                  fileResponse.FileConvertErrorType);
        }
    }
}
