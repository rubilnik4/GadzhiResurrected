using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Implementations.Converters;
using GadzhiWord.Word.Interfaces;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
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
        public IApplicationWord ApplicationWord { get; }

        public DocumentWord(Document document, IApplicationWord applicationWord)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            ApplicationWord = applicationWord ?? throw new ArgumentNullException(nameof(applicationWord));
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
        private OrientationType? _orientationType;

        /// <summary>
        /// Формат
        /// </summary>
        private OrientationType OrientationType => 
            _orientationType ??= _document.PageSetup.Orientation == WdOrientation.wdOrientLandscape 
                                 ? OrientationType.Landscape 
                                 : OrientationType.Portrait;

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void Save() => _document.Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void SaveAs(string filePath) => _document.SaveAs(filePath);

        /// <summary>
        /// Команда печати
        /// </summary>
        public IResultApplication PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize) =>
            new ResultApplication().
            Void(_ => ApplicationWord.PrintCommand());

        /// <summary>
        /// Экспорт файла
        /// </summary>      
        public string Export(string filePath) => filePath;

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
        public void CloseApplication() => ApplicationWord.CloseApplication();
    }
}
