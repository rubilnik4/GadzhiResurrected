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
        public static IReadOnlyList<IErrorCommon> IsFileDataRequestValid(FileDataRequestBase fileDataRequest)
        {
            var errors = new List<IErrorCommon>();

            string fileName = Path.GetFileNameWithoutExtension(fileDataRequest?.FilePath);
            string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(fileDataRequest?.FilePath);

            bool isValidName = !String.IsNullOrWhiteSpace(fileName);
            bool isValidExtension = ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension);
            bool isValidDataSource = fileDataRequest?.FileDataSource != null;

            if (!isValidName)
            {
                errors.Add(new ErrorCommon(ErrorConvertingType.IncorrectFileName, $"Некорректное имя файла {fileName}"));
            }
            if (!isValidExtension)
            {
                errors.Add(new ErrorCommon(ErrorConvertingType.IncorrectExtension, $"Некорректное расширение файла {fileExtension}"));
            }
            if (!isValidDataSource)
            {
                errors.Add(new ErrorCommon(ErrorConvertingType.IncorrectDataSource, $"Некорректные входные данные конвертации"));
            }

            return errors;
        }
    }
}