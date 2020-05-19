using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.Resources
{
    /// <summary>
    /// Ресурсы, используемые модулем конвертации
    /// </summary>
    public abstract class ResourcesApplication
    {
        protected ResourcesApplication(SignaturesLibrarySearching signaturesLibrarySearching)
        {
            SignaturesLibrarySearching = signaturesLibrarySearching ?? throw new ArgumentNullException(nameof(signaturesLibrarySearching));
        }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        public SignaturesLibrarySearching SignaturesLibrarySearching { get; }
    }
}
