using System;
using System.IO;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
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
        /// Путь к сохранению файла печати с учетом принципа именования
        /// </summary>
        public static IResultValue<string> GetFilePathByNamingType(string filePath, PdfNamingType pdfNamingType, IStamp stamp) =>
            pdfNamingType switch
            {
                PdfNamingType.ByFile => ChangeFilePathName(filePath, GetFileNameByIndex(filePath, stamp)),
                PdfNamingType.ByCode => ChangeFilePathName(filePath, GetFileNameByCode(stamp)),
                PdfNamingType.BySheet => ChangeFilePathName(filePath, GetFileNameBySheet(stamp)),
                _ => throw new ArgumentOutOfRangeException(nameof(pdfNamingType), pdfNamingType, null)
            };

        /// <summary>
        /// Получить путь к файлу с новым именем
        /// </summary>
        private static IResultValue<string> ChangeFilePathName(string filePath, IResultValue<string> fileNameChanged) =>
            fileNameChanged.
            ResultValueOk(fileName => FileSystemOperations.ChangeFilePathName(filePath, fileName));

        /// <summary>
        /// Получить имя файла с учетом количества штампов
        /// </summary>
        private static IResultValue<string> GetFileNameByIndex(string filePath, IStamp stamp) =>
            new ResultValue<string>(Path.GetFileNameWithoutExtension(filePath) +
                                    stamp.StampSettings.Id.ToFilePathPrefix());

        /// <summary>
        /// Получить имя файла согласно шифру
        /// </summary>
        private static IResultValue<string> GetFileNameByCode(IStamp stamp) =>
            stamp.StampBasicFields.
            ResultValueOk(basicFields => basicFields.FullCode.Text).
            ResultValueContinue(fullCode => !String.IsNullOrWhiteSpace(fullCode),
                okFunc: fullCode => fullCode,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.FieldNotFound, "Пустое поле шифра")).
            ToResultValueFromApplication();

        /// <summary>
        /// Получить имя файла согласно номеру листа
        /// </summary>
        private static IResultValue<string> GetFileNameBySheet(IStamp stamp) =>
            stamp.StampBasicFields.
            ResultValueOk(basicFields => basicFields.CurrentSheet.Text).
            ResultValueContinue(currentSheet => !String.IsNullOrWhiteSpace(currentSheet),
                okFunc: currentSheet => currentSheet,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.FieldNotFound, "Пустое поле номера листа")).
            ToResultValueFromApplication();
    }
}