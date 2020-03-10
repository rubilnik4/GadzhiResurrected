using ConvertingModels.Models.Interfaces.ApplicationLibrary.Application;
using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiWord.Word.Implementations.Converters;
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
    public partial class DocumentWord : IDocumentLibrary
    {
        /// <summary>
        /// Экземпляр файла
        /// </summary>
        private readonly Document _document;
      
        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        private readonly IApplicationLibrary _applicationWord;

        public DocumentWord(Document document, IApplicationLibrary applicationWord)
        {
            _document = document;
            _applicationWord = applicationWord;
        }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FullName => _document?.FullName;

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => _document != null;

        /// <summary>
        /// Формат
        /// </summary>
        public string PaperSize => WordPaperSizeToString.ConvertingPaperSizeToString(_document.PageSetup.PaperSize);

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void Save() => _document.Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public void SaveAs(string filePath) => _document.SaveAs(filePath);

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
    }
}
