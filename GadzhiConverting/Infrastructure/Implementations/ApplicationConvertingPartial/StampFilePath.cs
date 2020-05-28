using System;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;

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
        public static string GetFilePathByNamingType(string filePath, PdfNamingType pdfNamingType, IStamp stamp) =>
            pdfNamingType switch
            {
                PdfNamingType.ByFile => FileSystemOperations.ChangeFilePathName(filePath, GetFileNameByIndex(filePath, stamp)),
                PdfNamingType.ByCode => FileSystemOperations.ChangeFilePathName(filePath, GetFileNameByCode(filePath, stamp)),
                PdfNamingType.BySheet => FileSystemOperations.ChangeFilePathName(filePath, GetFileNameBySheet(filePath, stamp)),
                _ => throw new ArgumentOutOfRangeException(nameof(pdfNamingType), pdfNamingType, null)
            };

        /// <summary>
        /// Получить имя файла с учетом количества штампов
        /// </summary>
        private static string GetFileNameByIndex(string filePath, IStamp stamp) =>
            Path.GetFileNameWithoutExtension(filePath) + stamp.StampSettings.Id.ToFilePathPrefix();

        /// <summary>
        /// Получить имя файла согласно шифру
        /// </summary>
        private static string GetFileNameByCode(string filePath, IStamp stamp) =>
            stamp.StampBasicFields.
            WhereContinue(resultBasic => resultBasic.OkStatus && !String.IsNullOrWhiteSpace(resultBasic.Value.FullCode.Text),
                okFunc: resultBasic => resultBasic.Value.FullCode.Text,
                badFunc: resultBasic => GetFileNameByIndex(filePath, stamp));

        /// <summary>
        /// Получить имя файла согласно номеру листа
        /// </summary>
        private static string GetFileNameBySheet(string filePath, IStamp stamp) =>
            stamp.StampBasicFields.
            WhereContinue(resultBasic => resultBasic.OkStatus && !String.IsNullOrWhiteSpace(resultBasic.Value.CurrentSheet.Text),
                okFunc: resultBasic => resultBasic.Value.CurrentSheet.Text,
                badFunc: resultBasic => GetFileNameByIndex(filePath, stamp));
    }
}