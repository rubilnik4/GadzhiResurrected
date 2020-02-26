using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiConverting.Models.Implementations;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
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
            new FileDataMicrostation(fileDataServer.FilePathServer, fileDataServer.FilePathClient, ConvertColorPrint(fileDataServer.ColorPrint)) :
            throw new ArgumentNullException(nameof(fileDataServer));

        /// <summary>
        /// Конвертировать параметры принтеров
        /// </summary>
        public static PrintersInformationMicrostation PrintersServerToMicrostation(PrintersInformation printersInformation) =>
            new PrintersInformationMicrostation(PrinterServerToMicrostation(printersInformation?.PrintersPdf.FirstOrDefault()));           

        /// <summary>
        /// Преобразовать типы цвета печати
        /// </summary>       
        private static ColorPrintMicrostation ConvertColorPrint(ColorPrint colorPrint)
        {
            if (Enum.TryParse(colorPrint.ToString(), out ColorPrintMicrostation colorPrintMicrostation))
            {
                return colorPrintMicrostation;
            }
            else
            {
                throw new FormatException(nameof(colorPrint));
            }
        }

        private static PrinterInformationMicrostation  PrinterServerToMicrostation(PrinterInformation printerInformation) =>
           new PrinterInformationMicrostation(printerInformation.Name, printerInformation.PrefixSearchPaperSize);
    }
}
