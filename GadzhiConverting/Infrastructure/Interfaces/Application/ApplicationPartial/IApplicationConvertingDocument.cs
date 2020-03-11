using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Application.ApplicationPartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public interface IApplicationConvertingDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        IDocumentLibrary OpenDocument(string filePath);


        /// <summary>
        /// Сохранить документ
        /// </summary>
        IFileDataSourceServer SaveDocument(string filePath);


        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        IEnumerable<IFileDataSourceServer> CreatePdfFile(string filePath, ColorPrint colorPrint, string pdfPrinterName);

        /// <summary>
        /// Закрыть файл
        /// </summary>
        void CloseDocument();
    }
}
