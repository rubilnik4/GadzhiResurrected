using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Ресурсы, используемые модулем Microstation
    /// </summary>
    public class MicrostationResources
    {
        public MicrostationResources(string signatureMicrostationFileName, string stampMicrostationFileName)
        {           
            SignatureMicrostationFileName = signatureMicrostationFileName;
            StampMicrostationFileName = stampMicrostationFileName;
        }       

        /// <summary>
        /// Подписи для Microstation
        /// </summary>
        public string SignatureMicrostationFileName { get; }

        /// <summary>
        /// Штампы для Microstation
        /// </summary>
        public string StampMicrostationFileName { get; }
    }
}
