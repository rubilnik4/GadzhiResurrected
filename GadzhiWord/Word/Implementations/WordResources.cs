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
        public WordResources(SignaturesLibrarySearching signaturesLibrarySearching)
        {
            SignaturesLibrarySearching = signaturesLibrarySearching ?? throw new ArgumentNullException(nameof(signaturesLibrarySearching));          
        }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        public SignaturesLibrarySearching SignaturesLibrarySearching { get; }      
    }
}
