using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.StampCollections
{
    /// <summary>
    /// Контейнер штампа, содержащий составные части
    /// </summary>
    public class StampContainer
    {
        public StampContainer ()
        {
            StampMain = new StampMain();
        }
        
        /// <summary>
        /// Основной штамп
        /// </summary>
        public StampMain StampMain { get; }
    }
}
