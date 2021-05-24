using System;
using System.Collections.Generic;
using System.IO;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
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
        public static IResultError IsFileDataRequestValid(FileDataRequestBase fileDataRequest)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileDataRequest?.FilePath);
            string fileExtension = FilePathOperations.ExtensionWithoutPointFromPath(fileDataRequest?.FilePath);

            bool isValidName = !String.IsNullOrWhiteSpace(fileName);
            bool isValidExtension = ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension);
            bool isValidDataSource = fileDataRequest?.FileDataSource != null;

            var errors = new List<IErrorCommon>();
            if (!isValidName)
                errors.Add(new ErrorCommon(ErrorConvertingType.IncorrectFileName, $"Некорректное имя файла {fileName}"));
            
            if (!isValidExtension)
                errors.Add(new ErrorCommon(ErrorConvertingType.IncorrectExtension, $"Некорректное расширение файла {fileExtension}"));
            
            if (!isValidDataSource)
                errors.Add(new ErrorCommon(ErrorConvertingType.IncorrectDataSource, $"Некорректные входные данные конвертации"));
            
            return new ResultError(errors);
        }
    }
}