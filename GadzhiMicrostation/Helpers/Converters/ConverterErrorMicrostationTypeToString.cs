using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;

namespace GadzhiMicrostation.Helpers.Converters
{
    public static class ConvertErrorTypeToString
    {
        /// <summary>
        /// Словарь типов ошибок в строком значении
        /// </summary>
        public static IDictionary<ErrorMicrostationType, string> ErrorMicrostationTypeToString =>
            new Dictionary<ErrorMicrostationType, string>
            {
                { ErrorMicrostationType.ApplicationNotLoad , "Ошибка загрузки приложения" },
                { ErrorMicrostationType.FileNotOpen , "Невозможно открыть файл" },
                { ErrorMicrostationType.FileNotSaved , "Невозможно сохранить файл" },
                { ErrorMicrostationType.StampNotFound , "Штампы не найдены" },
                { ErrorMicrostationType.FileNotFound , "Файл не найден" },
                { ErrorMicrostationType.RangeNotValid , "Некорректный диапазон" },
                { ErrorMicrostationType.PrinterNotInstall , "Принтер не установлен" },
                { ErrorMicrostationType.PaperSizeNotFound  , "Формат принтера не найден" },
                { ErrorMicrostationType.PdfPrintingError  , "Ошибка печати PDF" },
                { ErrorMicrostationType.DwgCreatingError  , "Ошибка создания DWG" },
                { ErrorMicrostationType.SignatureNotFound  , "Подпись не найдена" },
                { ErrorMicrostationType.IncorrectExtension , "Некорректное расширение" },
                { ErrorMicrostationType.ArgumentNullReference , "Аргумент не задан" },
                { ErrorMicrostationType.NullReference , "Переменная не задана" },              
                { ErrorMicrostationType.UnknownError, "Неизвестная ошибка" },
            };

        /// <summary>
        /// Преобразовать тип ошибки в строковое значение
        /// </summary>       
        public static string ConvertErrorMicrostationTypeToString(ErrorMicrostationType errorMicrostationType)
        {
            string errorMicrostationTypeString = String.Empty;
            ErrorMicrostationTypeToString?.TryGetValue(errorMicrostationType, out errorMicrostationTypeString);

            return errorMicrostationTypeString;
        }
    }
}
