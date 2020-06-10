using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Infrastructure.Implementations.Converters.Errors
{
    /// <summary>
    /// Операции над типами ошибок
    /// </summary>
    public static class ConverterErrorType
    {
        /// <summary>
        /// Словарь типов ошибок в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<FileConvertErrorType, string> ErrorTypeString =>
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
                { FileConvertErrorType.FieldNotFound , "Поле не найдено" },
                { FileConvertErrorType.FileNotOpen , "Невозможно открыть файл" },
                { FileConvertErrorType.FileNotSaved , "Невозможно сохранить файл" },
                { FileConvertErrorType.StampNotFound , "Штампы не найдены" },
                { FileConvertErrorType.TableNotFound , "Таблицы не найдены" },
                { FileConvertErrorType.RangeNotValid , "Некорректный диапазон" },
                { FileConvertErrorType.PrinterNotInstall , "Принтер не установлен" },
                { FileConvertErrorType.PaperSizeNotFound  , "Формат принтера не найден" },
                { FileConvertErrorType.PdfPrintingError  , "Ошибка печати PDF" },
                { FileConvertErrorType.ExportError  , "Ошибка экспортирования файла" },
                { FileConvertErrorType.SignatureNotFound  , "Подпись не найдена" },
            };

        /// <summary>
        /// Преобразовать тип ошибки в строковое значение
        /// </summary>       
        public static string ErrorTypeToString(FileConvertErrorType fileConvertErrorType)
        {
            ErrorTypeString.TryGetValue(fileConvertErrorType, out string fileConvertErrorTypeString);
            return fileConvertErrorTypeString;
        }

        /// <summary>
        /// Определить наличие ошибок
        /// </summary>
        public static StatusError ErrorsTypeToStatusError(IEnumerable<FileConvertErrorType> fileConvertErrorsType)
        {
            var fileConvertErrorsTypeCollection = fileConvertErrorsType?.ToList() ?? new List<FileConvertErrorType>();

            return (fileConvertErrorsTypeCollection.Count == 0)
                ? StatusError.NoError
                : StatusError.CriticalError;
        }
    }
}
