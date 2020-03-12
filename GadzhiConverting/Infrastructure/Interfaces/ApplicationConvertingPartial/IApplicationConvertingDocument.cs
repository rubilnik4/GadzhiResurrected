using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial
{
    /// <summary>
    /// Подкласс приложения для работы с документом
    /// </summary>
    public interface IApplicationConvertingDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        ErrorConverting OpenDocument(string filePath);


        /// <summary>
        /// Сохранить документ
        /// </summary>
        (IFileDataSourceServer, ErrorConverting) SaveDocument(string filePath);


        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) CreatePdfFile(string filePath, ColorPrint colorPrint, string pdfPrinterName);

        /// <summary>
        /// Закрыть файл
        /// </summary>
        ErrorConverting CloseDocument();
    }
}
