using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Файл файл Microstation
    /// </summary>
    public partial class DocumentMicrostation : IDocumentLibraryElements
    {
        /// <summary>
        /// Найти таблицы-штампы во всех моделях и листах
        /// </summary>
        public IEnumerable<IStamp> FindStamps() => ModelsMicrostation.SelectMany(model => model.FindStamps());
    }
}
