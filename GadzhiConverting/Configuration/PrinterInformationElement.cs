using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name => this[nameof(Name)] as string;

        /// <summary>
        /// Параметр поиска форматов печати
        /// </summary>
        [ConfigurationProperty(nameof(PrefixSearchPaperSize), IsRequired = false)]
        public string PrefixSearchPaperSize => this[nameof(PrefixSearchPaperSize)] as string;

    }
}
