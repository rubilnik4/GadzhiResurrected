using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Models.Interfaces.Printers;
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
        (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) SaveDocument(string filePath);


        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) CreatePdfFile(string filePath, ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation);

        /// <summary>
        /// Закрыть файл
        /// </summary>
        ErrorConverting CloseDocument();
    }
}
