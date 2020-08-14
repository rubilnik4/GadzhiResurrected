using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Configuration
{
    /// <summary>
    /// Внесение данных о принтерах в конфигурационный файл
    /// </summary>
    public class RegisterPrintersInformationSection : ConfigurationSection
    {
        /// <summary>
        /// Имя секции в конфигурационном файле
        /// </summary>
        private const string SECTION_NAME = "PrintersInformationSection";

        public static RegisterPrintersInformationSection GetConfig() =>
            (RegisterPrintersInformationSection)ConfigurationManager.GetSection(SECTION_NAME)
            ?? new RegisterPrintersInformationSection();
        

        /// <summary>
        /// Коллекция PDF принтеров
        /// </summary>
        [ConfigurationProperty(nameof(PrintersPdfCollection))]
        public PrintersPdfCollection PrintersPdfCollection => this[nameof(PrintersPdfCollection)] as PrintersPdfCollection;
    }
}
