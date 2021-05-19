using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.Printers;

namespace GadzhiConverting.Configuration
{
    /// <summary>
    /// Конфигурационный файл. Параметры принтера
    /// </summary>
    public class PrinterInformationElement : ConfigurationElement
    {
        /// <summary>
        /// Имя принтера
        /// </summary>
        [ConfigurationProperty(nameof(Name), IsRequired = true)]
        public string Name =>
            this[nameof(Name)] as string;

        /// <summary>
        /// Имя принтера
        /// </summary>
        [ConfigurationProperty(nameof(PrinterType), IsRequired = true)]
        public PrinterType? PrinterType =>
            this[nameof(PrinterType)] as PrinterType?;

        /// <summary>
        /// Имя принтера
        /// </summary>
        [ConfigurationProperty(nameof(PrinterFormatType), IsRequired = true)]
        public PrinterFormatType? PrinterFormatType =>
            this[nameof(PrinterFormatType)] as PrinterFormatType?;

        /// <summary>
        /// Параметр поиска форматов печати
        /// </summary>
        [ConfigurationProperty(nameof(PrefixSearchPaperSize), IsRequired = false)]
        public string PrefixSearchPaperSize => 
            this[nameof(PrefixSearchPaperSize)] as string;

    }
}
