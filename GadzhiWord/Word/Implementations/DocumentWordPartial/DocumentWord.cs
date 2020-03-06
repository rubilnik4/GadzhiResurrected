using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Models.Implementations.FilesConvert;
using GadzhiWord.Models.Interfaces.FilesConvert;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
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
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        public DocumentWord(Document document, IMessagingService messagingService)
        {
            _document = document;
            _messagingService = messagingService;
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
            if (StampWord.IsValid)
            {
                return StampWord.Stamps?.Select(stamp => CreatePdfWithSignatures(stamp, filePath));
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.StampNotFound,
                                                                  $"Штампы в файле {Path.GetFileName(filePath)} не найдены"));
                return null;
            }
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private FileDataSourceServerWord CreatePdfWithSignatures(IStamp stamp, string filePath)
        {
            _messagingService.ShowAndLogMessage($"Обработка штампа {stamp.Name}");
            //stamp.CompressFieldsRanges();

            //stamp.DeleteSignaturesPrevious();
            InsertStampSignatures();

            _messagingService.ShowAndLogMessage($"Создание PDF для штампа {stamp.Name}");
            //FileDataSourceMicrostation fileDataSourceMicrostation = CreatePdfByStamp(stamp, filePath);

          //  DeleteStampSignatures();

            return null;
        }
    }
}
