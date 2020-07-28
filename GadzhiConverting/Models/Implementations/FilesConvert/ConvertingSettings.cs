using System;
using System.Linq;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(string personId, PdfNamingType pdfNamingType)
        {
            PersonId = personId ?? String.Empty;
            PdfNamingType = pdfNamingType;
        }

        /// <summary>
        /// Идентификатор личной подпись
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Принцип именование PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; }

        /// <summary>
        /// Информация о принтере PDF
        /// </summary>
        public IResultValue<IPrinterInformation> PdfPrinterInformation =>
            new ResultValue<IPrinterInformation>(PrintersInformation.PrintersPdf.FirstOrDefault(),
                                                 new ErrorCommon(ErrorConvertingType.PrinterNotInstall, "PDF принтер не найден"));

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        private static IPrintersInformation PrintersInformation => ProjectSettings.PrintersInformation;
    }
}