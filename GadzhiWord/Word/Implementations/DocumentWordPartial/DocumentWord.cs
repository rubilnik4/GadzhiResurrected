using ConvertingModels.Models.Enums;
using ConvertingModels.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiWord.Models.Implementations.FilesConvert;
using GadzhiWord.Models.Interfaces.FilesConvert;
using GadzhiWord.Word.Implementations.Converters;
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
        //private readonly IMessagingService _messagingService;

        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        private readonly IApplicationWord _applicationWord;

        public DocumentWord(Document document, IApplicationWord applicationWord)
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

        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        public IEnumerable<IFileDataSourceServerWord> CreatePdfInDocument(string filePath, ColorPrint colorPrint)
        {
            if (StampWord.IsValid)
            {
                return StampWord.Stamps?.Where(stamp => stamp.StampType == StampType.Main).
                                         Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint));
            }
            else
            {
                //_messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.StampNotFound,
                //                                                  $"Штампы в файле {Path.GetFileName(filePath)} не найдены"));
                return null;
            }
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private IFileDataSourceServerWord CreatePdfWithSignatures(IStamp stamp, string filePath, ColorPrint colorPrint)
        {
            //_messagingService.ShowAndLogMessage($"Обработка штампа {stamp.Name}");
            //stamp.CompressFieldsRanges();

            InsertStampSignatures();

            //_messagingService.ShowAndLogMessage($"Создание PDF для штампа {stamp.Name}");
            IFileDataSourceServerWord fileDataSourceServerWord = CreatePdfByStamp(stamp, filePath, colorPrint);

            DeleteStampSignatures();

            return fileDataSourceServerWord;
        }

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private IFileDataSourceServerWord CreatePdfByStamp(IStamp stamp, string filePath, ColorPrint colorPrint)
        {
            if (stamp != null)
            {
                return CreatePdf(filePath, colorPrint, stamp.PaperSize);
            }
            else
            {
                throw new ArgumentNullException(nameof(stamp));
            }
        }


        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private IFileDataSourceServerWord CreatePdf(string filePath, ColorPrint colorPrint, string paperSize)
        {
            string pdfPrinterName = _applicationWord.SetDefaultPdfPrinter();
            if (!String.IsNullOrWhiteSpace(pdfPrinterName))
            {
                _applicationWord.PrintPdfCommand(filePath);
                return new FileDataSourceServerWord(filePath, FileExtention.pdf, paperSize, pdfPrinterName);
            }
            return null;
        }
    }
}
