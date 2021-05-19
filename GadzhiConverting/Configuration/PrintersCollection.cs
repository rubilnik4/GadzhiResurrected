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
    [ConfigurationCollection(typeof(PrintersCollection), AddItemName = nameof(PrinterInformationElement))]
    public class PrintersCollection : ConfigurationElementCollection, IEnumerable<PrinterInformationElement>
    {
        /// <summary>
        /// Получить элемент по индексу
        /// </summary>       
        public PrinterInformationElement this[int index]
        {
            get => BaseGet(index) as PrinterInformationElement;           
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Получить элемент по ключу
        /// </summary>    
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

        /// <summary>
        /// Создать новый элемент
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement() => new PrinterInformationElement();
        
        /// <summary>
        /// Получить ключ
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element) => (element as PrinterInformationElement)?.Name 
                                                                                 ?? String.Empty;

        /// <summary>
        /// Перечисление
        /// </summary>  
        public new IEnumerator<PrinterInformationElement> GetEnumerator()=>
            BaseGetAllKeys().Select(key => (PrinterInformationElement)BaseGet(key)).GetEnumerator();
    }
}
