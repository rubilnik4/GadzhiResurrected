using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Implementations.Converting
{
    /// <summary>
    /// Обработка Штампа
    /// </summary>
    public class StampProcessing
    {
        /// <summary>
        /// Обработка штампа
        /// </summary>       
        public static void ConvertingStamp(IStamp stamp)
        {
            //stamp.CompressFieldsRanges();

            stamp.InsertSignatures();
        }
    }
}
