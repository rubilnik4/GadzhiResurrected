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
        [ConfigurationProperty(nameof(Name), IsRequired = true)]
        public string Name => this[nameof(Name)] as string;
        
    }
}
