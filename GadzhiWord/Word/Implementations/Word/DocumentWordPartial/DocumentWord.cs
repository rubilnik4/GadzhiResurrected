using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiWord.Models.Implementations.Specification;
using GadzhiWord.Word.Implementations.Converters;
using GadzhiWord.Word.Implementations.Excel;
using GadzhiWord.Word.Implementations.Excel.Export;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using GadzhiWord.Word.Interfaces.Word;
using GadzhiWord.Word.Interfaces.Word.Elements;
using Microsoft.Office.Interop.Word;

namespace GadzhiWord.Word.Implementations.Word.DocumentWordPartial
{
    /// <summary>
    /// Документ Word
    /// </summary>
    public partial class DocumentWord : IDocumentWord
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly Document _document;

        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        public IApplicationOffice ApplicationOffice { get; }

        public DocumentWord(Document document, IApplicationOffice applicationOffice)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            ApplicationOffice = applicationOffice ?? throw new ArgumentNullException(nameof(applicationOffice));
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _fullName;

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FullName => _fullName ??= _document?.FullName;

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => _document != null;

        /// <summary>
        /// Формат
        /// </summary>
        private string _paperSize;

        /// <summary>
        /// Формат
        /// </summary>
        private string PaperSize => _paperSize ??= WordPaperSizeToString.PaperSizeToString(_document.PageSetup.PaperSize);

        /// <summary>
        /// Формат
        /// </summary>
        private StampOrientationType? _orientationType;

        /// <summary>
        /// Формат
        /// </summary>
        private StampOrientationType OrientationType =>
            _orientationType ??= _document.PageSetup.Orientation == WdOrientation.wdOrientLandscape
                                 ? StampOrientationType.Landscape
                                 : StampOrientationType.Portrait;

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void Save() => _document.Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public IResultApplication SaveAs(string filePath) =>
            Path.GetExtension(filePath).
            WhereContinue(fileExtension => ValidFileExtensions.IsFileExtensionEqual(fileExtension, FileExtension.Docx),
            okFunc: fileExtension => new ResultApplication().
                                         ResultVoidOk(_ => _document.SaveAs(filePath)).
                                         ToResultApplication(),
            badFunc: fileExtension => new ResultApplication(new ErrorApplication(ErrorApplicationType.IncorrectExtension,
                                                                                 $"Некорректное расширение {fileExtension} для файла типа docx")));

        /// <summary>
        /// Команда печати
        /// </summary>
        public IResultApplication PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize) =>
            new ResultApplication().
            Void(_ => ApplicationOffice.PrintCommand());

        /// <summary>
        /// Экспорт файла
        /// </summary>      
        public IResultAppValue<string> Export(string filePath) =>
            new ResultAppCollection<ITableElementWord>(GetTables()).
            ResultValueOk(tables => tables.Where(ValidatingTableWord.IsTableSpecification)).
            ToResultCollection().
            ResultValueContinue(tablesSpecification => tablesSpecification.Count > 0,
                okFunc: tables => tables,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Таблицы спецификации не найдены")).
            ResultValueOkBind(tablesWord => ExportWordTableToExcel(tablesWord, filePath));

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        public void Close() => _document.Close();

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        public void CloseWithSaving()
        {
            Save();
            Close();
        }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        public void CloseApplication() => ApplicationOffice.CloseApplication();

        /// <summary>
        /// Экспортировать данные таблицы Word в Excel
        /// </summary>
        private IResultAppValue<string> ExportWordTableToExcel(IEnumerable<ITableElementWord> tablesWord, string filePath) =>
            new ResultAppValue<IBookExcel>(ApplicationOffice.CreateWorkbook()).
            ResultValueOkBind(book => ExportTableFromWord.ExportTable(book, tablesWord, filePath).
                                      Void(_ => book.Close()));
    }
}
