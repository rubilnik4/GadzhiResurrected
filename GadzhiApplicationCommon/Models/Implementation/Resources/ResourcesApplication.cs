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
        protected ResourcesApplication(SignaturesSearching signaturesSearching)
        {
            SignaturesSearching = signaturesSearching ?? throw new ArgumentNullException(nameof(signaturesSearching));
        }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        public SignaturesSearching SignaturesSearching { get; }
    }
}
