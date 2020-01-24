﻿using GadzhiDTO.FilesConvertDTO.Classes;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters.DTO
{
    public static class FileDataToDTOConverter
    {
        /// <summary>
        /// Конвертер информации о файле из локальной модели в трансферную
        /// </summary>      
        public static FileDataDTO ConvertToFileDataDTO(FileData fileData)
        {
            return new FileDataDTO()
            {
                ColorPrint = fileData.ColorPrint,
                FileName = fileData.FileName,
                FilePath = fileData.FilePath,
                FileType = fileData.FileType,
                StatusProcessing = fileData.StatusProcessing,
            };
        }
    }
}
