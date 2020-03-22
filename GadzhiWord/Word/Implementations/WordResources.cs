using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Word.Implementations
{
    /// <summary>
    /// Ресурсы, используемые модулем Word
    /// </summary>
    public class WordResources
    {
        public WordResources(string signatureWordFileName)
        {
            SignatureWordFileName = signatureWordFileName;          
        }

        /// <summary>
        /// Подписи для Word
        /// </summary>
        public string SignatureWordFileName { get; }      
    }
}
