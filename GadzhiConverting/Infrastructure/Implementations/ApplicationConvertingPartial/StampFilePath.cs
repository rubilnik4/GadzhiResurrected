using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;

namespace GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial
{
    /// <summary>
    /// Генерация пути для сохранения файлов со штампами
    /// </summary>
    public static class StampFilePath
    {
        /// <summary>
        /// Получить имена файлов с учетом принципа именования и индексацией
        /// </summary>
        public static IResultCollection<string> GetFileNamesByNamingType(IEnumerable<IStamp> stamps, string fileName, PdfNamingType pdfNamingType) =>
            stamps.Select(stamp => GetFileNameByNamingType(fileName, pdfNamingType, stamp)).
            ToResultCollection().
            ResultValueOk(fileNames => AddIndexesToDuplicateFileNames(fileNames, FindDuplicates(fileNames))).
            ToResultCollection();

        /// <summary>
        /// Имя файла печати с учетом принципа именования
        /// </summary>
        private static IResultValue<string> GetFileNameByNamingType(string fileName, PdfNamingType pdfNamingType, IStamp stamp) =>
            pdfNamingType switch
            {
                PdfNamingType.ByFile => GetFileNameByIndex(fileName, stamp),
                PdfNamingType.ByCode => GetFileNameByCode(stamp),
                PdfNamingType.BySheet => GetFileNameBySheet(stamp),
                _ => throw new ArgumentOutOfRangeException(nameof(pdfNamingType), pdfNamingType, null)
            };

        /// <summary>
        /// Получить имя файла с учетом количества штампов
        /// </summary>
        private static IResultValue<string> GetFileNameByIndex(string fileName, IStamp stamp) =>
            new ResultValue<string>(fileName + stamp.StampSettings.Id.ToFilePathPrefix());

        /// <summary>
        /// Получить имя файла согласно шифру
        /// </summary>
        private static IResultValue<string> GetFileNameByCode(IStamp stamp) =>
            stamp.StampBasicFields.FullCode.
            ResultValueOk(fullCodeField => FileSystemOperations.GetValidFileName(fullCodeField.Text)).
            ToResultValueFromApplication();

        /// <summary>
        /// Получить имя файла согласно номеру листа
        /// </summary>
        private static IResultValue<string> GetFileNameBySheet(IStamp stamp) =>
            stamp.StampBasicFields.CurrentSheetNumber.
            ResultValueOk(CurrentSheetFormat).
            ToResultValueFromApplication();

        /// <summary>
        /// Перевести номер листа в строковый формат вида ##
        /// </summary>
        private static string CurrentSheetFormat(int currentSheet) => (currentSheet < 10)
            ? $"{ currentSheet:00}"
            : $"{ currentSheet}";

        /// <summary>
        /// Найти дублирующие пути файлов
        /// </summary>
        private static IReadOnlyList<string> FindDuplicates(IEnumerable<string> filePaths) =>
            filePaths.
            GroupBy(filePath => filePath).
            Where(group => group.Count() > 1).
            Select(group => group.Key).
            ToList();

        /// <summary>
        /// Добавить индексы к дублирующимся именам
        /// </summary>
        private static IEnumerable<string> AddIndexesToDuplicateFileNames(IEnumerable<string> fileNames, IReadOnlyList<string> duplicatedNames)
        {
            var indexDictionary = duplicatedNames.ToDictionary(fileName => fileName, _ => 1);

            foreach (string fileName in fileNames)
            {
                if (!duplicatedNames.Contains(fileName))
                {
                    yield return fileName;
                }
                else
                {
                    string fileNameIndex = fileName + "_" + indexDictionary[fileName];
                    indexDictionary[fileName] += 1;
                    yield return fileNameIndex;
                }
            }
        }
    }
}