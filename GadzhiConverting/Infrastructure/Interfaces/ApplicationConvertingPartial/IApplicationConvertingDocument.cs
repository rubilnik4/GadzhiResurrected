using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiConverting.Models.Implementations.FilesConvert;

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
        IResultValue<IDocumentLibrary> OpenDocument(string filePath);

        /// <summary>
        /// Сохранить документ
        /// </summary>
        IResultValue<IFileDataSourceServer> SaveDocument(IDocumentLibrary documentLibrary, IFilePath filePath);

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        IResultCollection<IFileDataSourceServer> CreatePdfFile(IDocumentLibrary documentLibrary, IFilePath filePath,
                                                               ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation);

        /// <summary>
        /// Экспортировать файл
        /// </summary>
        IResultValue<IFileDataSourceServer> CreateExportFile(IDocumentLibrary documentLibrary, IFilePath filePath);

        /// <summary>
        /// Закрыть файл
        /// </summary>
        IResultError CloseDocument(IDocumentLibrary documentLibrary, string filePath);
    }
}
