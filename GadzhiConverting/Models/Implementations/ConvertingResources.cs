using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Ресурсы, используемые модулями конвертации
    /// </summary>
    public class ConvertingResources
    {
        public ConvertingResources(string signatureWordFileName, string signatureMicrostationFileName, string stampMicrostationFileName)
        {
            SignatureWordFileName = signatureWordFileName;
            SignatureMicrostationFileName = signatureMicrostationFileName;
            StampMicrostationFileName = stampMicrostationFileName;
        }

        /// <summary>
        /// Типовая подпись для Word
        /// </summary>
        public string SignatureWordFileName { get; }

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
