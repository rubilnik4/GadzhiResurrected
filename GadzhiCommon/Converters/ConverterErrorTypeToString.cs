using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiCommon.Converters
{
    /// <summary>
    /// Операции над типами ошибок
    /// </summary>
    public static class ConverterErrorType
    {
        /// <summary>
        /// Словарь типов ошибок в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<FileConvertErrorType, string> ErrorTypeToString =>
            new Dictionary<FileConvertErrorType, string>
            {
                { FileConvertErrorType.NoError , "Ошибка отсутствует" },
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
                { FileConvertErrorType.FormatException , "Формат преобразования задан не верно" },
                { FileConvertErrorType.AttemptingCount , "Превышено число попыток" },
                { FileConvertErrorType.InternalError , "Внутренняя ошибка" },
                { FileConvertErrorType.UnknownError, "Неизвестная ошибка" },

                { FileConvertErrorType.ApplicationNotLoad , "Ошибка загрузки приложения" },
                { FileConvertErrorType.LibraryNotFound , "Библиотека конвертации не найдена" },
                { FileConvertErrorType.FileNotOpen , "Невозможно открыть файл" },
                { FileConvertErrorType.FileNotSaved , "Невозможно сохранить файл" },
                { FileConvertErrorType.StampNotFound , "Штампы не найдены" },
                { FileConvertErrorType.RangeNotValid , "Некорректный диапазон" },
                { FileConvertErrorType.PrinterNotInstall , "Принтер не установлен" },
                { FileConvertErrorType.PaperSizeNotFound  , "Формат принтера не найден" },
                { FileConvertErrorType.PdfPrintingError  , "Ошибка печати PDF" },
                { FileConvertErrorType.ExportError  , "Ошибка экспортирования файла" },
                { FileConvertErrorType.SignatureNotFound  , "Подпись не найдена" },
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
            var fileConvertErrorTypeString = String.Empty;
            ErrorTypeToString?.TryGetValue(fileConvertErrorType, out fileConvertErrorTypeString);

            return fileConvertErrorTypeString;
        }

        public static StatusError FileErrorsTypeToStatusError(IEnumerable<FileConvertErrorType> fileConvertErrorsType)
        {
            var fileConvertErrorsTypeCollection = fileConvertErrorsType?.ToList() ?? new List<FileConvertErrorType>();
            bool hasCriticalErrors = fileConvertErrorsTypeCollection.Any(error => CriticalErrorType?.Contains(error) == true);
            bool hasInformationErrors = fileConvertErrorsTypeCollection.Any(error => InformationalErrorType?.Contains(error) == true);

            return (hasCriticalErrors, hasInformationErrors) switch
            {
                (false, true) => StatusError.InformationError,
                (true, _) => StatusError.CriticalError,
                (_, _) => StatusError.NoError,
            };
        }
    }
}
