using System.Collections.Generic;
using System.ComponentModel;
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
        public static IReadOnlyDictionary<ErrorConvertingType, string> ErrorTypeString =>
            new Dictionary<ErrorConvertingType, string>
            {
                { ErrorConvertingType.NoError , "Ошибка отсутствует" },
                { ErrorConvertingType.FileNotFound , "Файл не найден" },
                { ErrorConvertingType.IncorrectDataSource , "Некорректный файл данных" },
                { ErrorConvertingType.IncorrectExtension  , "Некорректное расширение файла" },
                { ErrorConvertingType.IncorrectFileName , "Некорректное имя файла" },
                { ErrorConvertingType.RejectToSave , "Файл не сохранен" },
                { ErrorConvertingType.AbortOperation , "Отмена операции" },
                { ErrorConvertingType.TimeOut, "Истекло время выполнения операции" },
                { ErrorConvertingType.Communication , "Связь с сервером не установлена" },
                { ErrorConvertingType.NullReference , "Переменная не задана" },
                { ErrorConvertingType.ArgumentNullReference , "Аргумент не задан" },
                { ErrorConvertingType.ArgumentOutOfRange , "Превышение допустимых пределов" },
                { ErrorConvertingType.ValueNotInitialized , "Переменная не инициализирована" },
                { ErrorConvertingType.FormatException , "Формат преобразования задан не верно" },
                { ErrorConvertingType.InvalidEnumArgumentException , "Ошибка перечисления" },
                { ErrorConvertingType.AttemptingCount , "Превышено число попыток" },
                { ErrorConvertingType.InternalError , "Внутренняя ошибка" },
                { ErrorConvertingType.UnknownError, "Неизвестная ошибка" },

                { ErrorConvertingType.ApplicationNotLoad , "Ошибка загрузки приложения" },
                { ErrorConvertingType.LibraryNotFound , "Библиотека конвертации не найдена" },
                { ErrorConvertingType.FieldNotFound , "Поле не найдено" },
                { ErrorConvertingType.FileNotOpen , "Невозможно открыть файл" },
                { ErrorConvertingType.FileNotSaved , "Невозможно сохранить файл" },
                { ErrorConvertingType.StampNotFound , "Штампы не найдены" },
                { ErrorConvertingType.TableNotFound , "Таблицы не найдены" },
                { ErrorConvertingType.RangeNotValid , "Некорректный диапазон" },
                { ErrorConvertingType.PrinterNotInstall , "Принтер не установлен" },
                { ErrorConvertingType.PaperSizeNotFound  , "Формат принтера не найден" },
                { ErrorConvertingType.PdfPrintingError  , "Ошибка печати PDF" },
                { ErrorConvertingType.ExportError  , "Ошибка экспортирования файла" },
                { ErrorConvertingType.SignatureNotFound  , "Подпись не найдена" },
            };

        /// <summary>
        /// Преобразовать тип ошибки в строковое значение
        /// </summary>       
        public static string ErrorTypeToString(ErrorConvertingType errorConvertingType)
        {
            ErrorTypeString.TryGetValue(errorConvertingType, out string fileConvertErrorTypeString);
            return fileConvertErrorTypeString;
        }

        /// <summary>
        /// Определить наличие ошибок
        /// </summary>
        public static StatusErrorType ErrorsTypeToStatusError(IEnumerable<ErrorConvertingType> fileConvertErrorsType)
        {
            var fileConvertErrorsTypeCollection = fileConvertErrorsType?.ToList() ?? new List<ErrorConvertingType>();

            return (fileConvertErrorsTypeCollection.Count == 0)
                ? StatusErrorType.NoError
                : StatusErrorType.CriticalError;
        }
    }
}
