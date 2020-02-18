﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GadzhiCommonServer.Infrastructure.Implementations
{
    /// <summary>
    /// Класс проверки входных данных
    /// </summary>
    public static class ValidateDTOData
    {
        /// <summary>
        /// Проверить целостность выходных данных для конвертации
        /// </summary>
        public static (bool isValid, IEnumerable<FileConvertErrorType> errors) IsFileDataRequestValid(FileDataRequestBase fileDataRequest)
        {
            var errors = new List<FileConvertErrorType>();

            string fileName = Path.GetFileNameWithoutExtension(fileDataRequest?.FilePath);
            string fileExtension = FileHelpers.ExtensionWithoutPointFromPath(fileDataRequest?.FilePath);

            bool isValidName = !String.IsNullOrWhiteSpace(fileName);
            bool isValidExtension = !String.IsNullOrWhiteSpace(fileExtension) &&
                                    ValidFileExtentions.DocAndDgnFileTypes.Contains(fileExtension);          
            bool isValidDataSource = fileDataRequest?.FileDataSource != null;

            if (!isValidName)
            {
                errors.Add(FileConvertErrorType.IncorrectFileName);
            }
            if (!isValidExtension)
            {
                errors.Add(FileConvertErrorType.IncorrectExtension);
            }
            if (!isValidDataSource)
            {
                errors.Add(FileConvertErrorType.IncorrectDataSource);
            }

            bool isValid = isValidName && isValidExtension && isValidDataSource;
            return (isValid, errors);
        }
    }
}