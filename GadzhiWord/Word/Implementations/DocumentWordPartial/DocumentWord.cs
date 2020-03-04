using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Models.Interfaces.FilesConvert;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using GadzhiWord.Word.Interfaces.StampPartial;
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
        private readonly IApplicationWord _applicationWord;

        public DocumentWord(Document document,
                            IApplicationWord applicationWord)
        {
            _document = document;
            _applicationWord = applicationWord;
        }

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => _document != null;

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IList<IStampWord> Stamps => FindStamps().ToList();

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

        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        public IEnumerable<IFileDataSourceServerWord> CreatePdfInDocument(string filePath)
        {
            if (Stamps.Any())
            {
                // return Stamps?.Select(stamp => CreatePdfWithSignatures(stamp, filePath));
                return null;
            }
            else
            {
                _applicationWord.MessagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.StampNotFound,
                                                                  $"Штампы в файле {Path.GetFileName(filePath)} не найдены"));
                return null;
            }
        }

    
    }
}
