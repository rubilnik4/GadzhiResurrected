using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiWord.Word.Implementations
{
    /// <summary>
    /// Ресурсы, используемые модулем Word
    /// </summary>
    public class WordResources
    {
        public WordResources(IReadOnlyList<SignatureLibrary> signatureNames)
        {
            SignatureNames = signatureNames;          
        }

        /// <summary>
        /// Имена для подписей
        /// </summary>
        public IReadOnlyList<SignatureLibrary> SignatureNames { get; }      
    }
}
