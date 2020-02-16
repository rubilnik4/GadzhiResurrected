using GadzhiMicrostation.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Helpers.Converters
{
    public static class ConverterErrorTypeToString
    {
        /// <summary>
        /// Словарь типов ошибок в строком значении
        /// </summary>
        public static IDictionary<ErrorMicrostationType, string> ErrorMicrostationTypeToString =
            new Dictionary<ErrorMicrostationType, string>
            {
                { ErrorMicrostationType.ApplicationLoad , "Ошибка загрузки приложения" },
                { ErrorMicrostationType.DesingFileOpen , "Некорректный файл" },
                { ErrorMicrostationType.UnknownError, "Неизвестная ошибка" },              
                { ErrorMicrostationType.NullReference , "Переменная не задана" },
                { ErrorMicrostationType.ArgumentNullReference , "Аргумент не задан" },
                { ErrorMicrostationType.FileNotFound , "Файл не найден" },
                { ErrorMicrostationType.IncorrectExtension , "Некорректное расширение" },
                { ErrorMicrostationType.StampNotFound , "Штампы не найдены" },
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
