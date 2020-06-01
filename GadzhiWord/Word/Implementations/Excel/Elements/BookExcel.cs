using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiWord.Extensions.Excel;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using Microsoft.Office.Interop.Excel;

namespace GadzhiWord.Word.Implementations.Excel.Elements
{
    public class BookExcel: IBookExcel
    {
        /// <summary>
        /// Экземпляр листа
        /// </summary>
        private readonly Workbook _workbook;

        public BookExcel(Workbook workbook)
        {
            _workbook = workbook ?? throw new ArgumentNullException(nameof(workbook));
        }

        /// <summary>
        /// Листы в книге
        /// </summary>
        private IReadOnlyList<ISheetExcel> _sheets;

        /// <summary>
        /// Листы в книге
        /// </summary>
        public IReadOnlyList<ISheetExcel> Sheets => _sheets ??= GetSheetsExcel();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public IResultAppValue<string> SaveAs(string filePath) =>
            Path.GetExtension(filePath).
            WhereContinue(fileExtension => ValidFileExtensions.IsFileExtensionEqual(fileExtension, FileExtension.Xlsx),
            okFunc: fileExtension => new ResultAppValue<string>(filePath).
                                     ResultVoidOk(_ => _workbook.SaveAs(filePath)),
            badFunc: fileExtension => new ResultAppValue<string>(new ErrorApplication(ErrorApplicationType.IncorrectExtension,
                                                                                      $"Некорректное расширение {fileExtension} для файла типа docx")));

        /// <summary>
        /// Закрыть
        /// </summary>
        public void Close() => _workbook.Close();

        /// <summary>
        /// Получить список листов
        /// </summary>
        private IReadOnlyList<ISheetExcel> GetSheetsExcel() =>
            _workbook.Sheets.ToIEnumerable().
            Select(sheet => new SheetExcel(sheet)).
            ToList();
    }
}