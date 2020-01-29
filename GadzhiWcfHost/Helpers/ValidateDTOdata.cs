using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Helpers
{
    /// <summary>
    /// Класс проверки входных данных
    /// </summary>
    public static class ValidateDTOData
    {
        /// <summary>
        /// Проверить целостность выходных данных для конвертации
        /// </summary>
        public static (bool isValid, IEnumerable<FileConvertErrorType> errors) IsFileDataRequestValid(FileDataRequest fileDataRequest)
        {
            var errors = new List<FileConvertErrorType>();

            bool isValidName = !String.IsNullOrWhiteSpace(fileDataRequest?.FileName);
            bool isValidExtension = !String.IsNullOrWhiteSpace(fileDataRequest?.FileExtension) &&
                                    ValidFileExtentions.DocAndDgnFileTypes.Contains(fileDataRequest?.FileExtension);
            bool isValidDataSource = fileDataRequest?.FileDataSource != null;

            if (isValidName)
            {
                errors.Add(FileConvertErrorType.IncorrectFileName);
            }
            if (isValidExtension)
            {
                errors.Add(FileConvertErrorType.IncorrectExtension);
            }
            if (isValidDataSource)
            {
                errors.Add(FileConvertErrorType.IncorrectDataSource);
            }            

            bool isValid = isValidName && isValidExtension && isValidDataSource;
            return (isValid, errors);
        }
    }
}