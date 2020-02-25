using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Configuration
{
    /// <summary>
    /// Конфигурационный файл. Параметры принтеров
    /// </summary>
    [ConfigurationCollection(typeof(PrintersPdfCollection), AddItemName = nameof(PrinterInformationElement))]
    public class PrintersPdfCollection : ConfigurationElementCollection
    {
        public PrinterInformationElement this[int index]
        {
            get => base.BaseGet(index) as PrinterInformationElement;           
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new PrinterInformationElement this[string responseString]
        {
            get =>  (PrinterInformationElement)BaseGet(responseString); 
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement() => new PrinterInformationElement();
        
        protected override object GetElementKey(ConfigurationElement element) => ((PrinterInformationElement)element)?.Name;       
    }
}
