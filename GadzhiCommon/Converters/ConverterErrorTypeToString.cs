using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiCommon.Converters
{
    public static class ConverterErrorType
    {
        /// <summary>
        /// Словарь типов ошибок в строком значении
        /// </summary>
        public static IReadOnlyDictionary<FileConvertErrorType, string> ErrorTypeToString =>
            new Dictionary<FileConvertErrorType, string>
            {
                { FileConvertErrorType.NoError , "Ошибка отсутсвует" },
                { FileConvertErrorType.FileNotFound , "Файл не найден" },
                { FileConvertErrorType.IncorrectDataSource , "Некорректный файл данных" },
                { FileConvertErrorType.IncorrectExtension  , "Некорректное расширение файла" },
                { FileConvertErrorType.IncorrectFileName , "Некорректное имя файла" },
                { FileConvertErrorType.RejectToSave , "Файл не сохранен" },
                { FileConvertErrorType.AbortOperation , "Отмена операции" },
                { FileConvertErrorType.TimeOut, "Время операции вышло" },
                { FileConvertErrorType.Communication , "Связь с сервером прервана" },
                { FileConvertErrorType.NullReference , "Переменная не задана" },
                { FileConvertErrorType.ArgumentNullReference , "Аргумент не задан" },
                { FileConvertErrorType.FormatException , "Формат парсинга задан не верно" },
                { FileConvertErrorType.AttemptingCount , "Превышено число попыток" },
                { FileConvertErrorType.InternalError , "Внутренняя ошибка" },
                { FileConvertErrorType.UnknownError, "Неизвестная ошибка" },

                { FileConvertErrorType.ApplicationNotLoad , "Ошибка загрузки приложения" },
                { FileConvertErrorType.FileNotOpen , "Невозможно открыть файл" },
                { FileConvertErrorType.FileNotSaved , "Невозможно сохранить файл" },
                { FileConvertErrorType.StampNotFound , "Штампы не найдены" },
                { FileConvertErrorType.RangeNotValid , "Некорректный диапазон" },
                { FileConvertErrorType.PrinterNotInstall , "Принтер не установлен" },
                { FileConvertErrorType.PaperSizeNotFound  , "Формат принтера не найден" },
                { FileConvertErrorType.PdfPrintingError  , "Ошибка печати PDF" },
                { FileConvertErrorType.DwgCreatingError  , "Ошибка создания DWG" },
                { FileConvertErrorType.SignatureNotFound  , "Подпись не найдена" },
                { FileConvertErrorType.ArgumentNullReference , "Аргумент не задан" },
            };

        /// <summary>
        /// Информационные ошибки
        /// </summary>
        public static IReadOnlyList<FileConvertErrorType> AllErrorType =>
           Enum.GetValues(typeof(FileConvertErrorType)).Cast<FileConvertErrorType>().ToList();

        /// <summary>
        /// Информационные ошибки
        /// </summary>
        public static IReadOnlyList<FileConvertErrorType> InformationalErrorType =>
            new List<FileConvertErrorType>()
            {
               FileConvertErrorType.SignatureNotFound,
            };

        /// <summary>
        /// Критические ошибки
        /// </summary>
        public static IReadOnlyList<FileConvertErrorType> CriticalErrorType =>
           AllErrorType?.Except(InformationalErrorType).
                         Where(error => error != FileConvertErrorType.NoError).ToList();

        /// <summary>
        /// Преобразовать тип ошибки в строковое значение
        /// </summary>       
        public static string FileErrorTypeToString(FileConvertErrorType fileConvertErrorType)
        {
            string fileConvertErrorTypeString = String.Empty;
            ErrorTypeToString?.TryGetValue(fileConvertErrorType, out fileConvertErrorTypeString);

            return fileConvertErrorTypeString;
        }

        public static StatusError FileErrorsTypeToStatusError(IEnumerable<FileConvertErrorType> fileConvertErrorsType)
        {
            bool hasCriticalErrors = fileConvertErrorsType?.Any(error => CriticalErrorType?.Contains(error) == true) ?? false;
            bool hasInformationErrors = fileConvertErrorsType?.Any(error => InformationalErrorType?.Contains(error) == true) ?? false;

            StatusError statusError = StatusError.NoError;
            if (hasInformationErrors && !hasCriticalErrors)
            {
                statusError = StatusError.InformationError;
            }
            else if (hasCriticalErrors)
            {
                statusError = StatusError.NoError;
            }

            return statusError;
        }
    }
}
