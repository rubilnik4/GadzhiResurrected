using ConvertingModels.Models.Interfaces.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Implementations.Printers;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.FilesData;
using GadzhiMicrostation.Models.Implementations.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертация файла серверной части в модуль Microstation
    /// </summary>
    public static class ConverterFileDataServerToMicrostation
    {
        /// <summary>
        /// Конвертировать файла серверной части в модуль Microstation
        /// </summary>
        public static FileDataMicrostation FileDataServerToMicrostation(FileDataServer fileDataServer) =>
            fileDataServer != null ?
            new FileDataMicrostation(fileDataServer.FilePathServer, fileDataServer.FilePathClient, 
                                     ConvertColorPrint(fileDataServer.ColorPrint)) :
            throw new ArgumentNullException(nameof(fileDataServer));

        /// <summary>
        /// Конвертировать параметры принтеров
        /// </summary>
        public static PrintersInformationMicrostation PrintersServerToMicrostation(PrintersInformation printersInformation) =>
            new PrintersInformationMicrostation(PrinterServerToMicrostation(printersInformation?.PrintersPdf.FirstOrDefault()));           

        /// <summary>
        /// Преобразовать типы цвета печати
        /// </summary>       
        private static ColorPrintMicrostation ConvertColorPrint(ColorPrint colorPrint) =>      
            Enum.TryParse(colorPrint.ToString(), out ColorPrintMicrostation colorPrintMicrostation) ?
            colorPrintMicrostation :            
            throw new FormatException(nameof(colorPrint));

        /// <summary>
        /// Преобразовать параметры принтера
        /// </summary>       
        private static PrinterInformationMicrostation  PrinterServerToMicrostation(IPrinterInformation printerInformation) =>
           new PrinterInformationMicrostation(printerInformation.Name, printerInformation.PrefixSearchPaperSize);
    }
}
