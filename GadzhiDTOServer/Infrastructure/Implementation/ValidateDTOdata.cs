using System;
using System.Collections.Generic;
using System.IO;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.Infrastructure.Implementation
{
    /// <summary>
    /// Класс проверки входных данных
    /// </summary>
    public static class ValidateDtoData
    {
        /// <summary>
        /// Проверить целостность выходных данных для конвертации
        /// </summary>
        public static (bool isValid, IEnumerable<FileConvertErrorType> errors) IsFileDataRequestValid(FileDataRequestBase fileDataRequest)
        {
            var errors = new List<FileConvertErrorType>();

            string fileName = Path.GetFileNameWithoutExtension(fileDataRequest?.FilePath);
            string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(fileDataRequest?.FilePath);

            bool isValidName = !String.IsNullOrWhiteSpace(fileName);
            bool isValidExtension = ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension);
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