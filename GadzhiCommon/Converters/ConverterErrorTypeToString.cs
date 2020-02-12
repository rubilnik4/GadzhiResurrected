﻿using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Converters
{
    public static class ConverterErrorTypeToString
    {
        /// <summary>
        /// Словарь типов ошибок в строком значении
        /// </summary>
        public static IReadOnlyDictionary<FileConvertErrorType, string> ErrorTypeToString =
            new Dictionary<FileConvertErrorType, string>
            {
                { FileConvertErrorType.AbortOperation , "Отмена операции" },
                { FileConvertErrorType.FileNotFound , "Файл не найден" },
                { FileConvertErrorType.IncorrectDataSource , "Некорректный файл данных" },
                { FileConvertErrorType.IncorrectExtension  , "Некорректное расширение файла" },
                { FileConvertErrorType.IncorrectFileName , "Некорректное имя файла" },
                { FileConvertErrorType.NoError , "Ошибка отсутсвует" },
                { FileConvertErrorType.RejectToSave , "Файл не сохранен" },
                { FileConvertErrorType.UnknownError, "Неизвестная ошибка" },
                { FileConvertErrorType.TimeOut, "Время операции вышло" },
                { FileConvertErrorType.Communication , "Связь с сепрвером прервана" },
                { FileConvertErrorType.NullReference , "Переменная не задана" },
                { FileConvertErrorType.ArgumentNullReference , "Аргумент не задан" },
            };

        /// <summary>
        /// Преобразовать тип ошибки в строковое значение
        /// </summary>       
        public static string ConvertFileConvertErrorTypeToString(FileConvertErrorType fileConvertErrorType)
        {
            string fileConvertErrorTypeString = String.Empty;
            ErrorTypeToString?.TryGetValue(fileConvertErrorType, out fileConvertErrorTypeString);

            return fileConvertErrorTypeString;
        }
    }
}
