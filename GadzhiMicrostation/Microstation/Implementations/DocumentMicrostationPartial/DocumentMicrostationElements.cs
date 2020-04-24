using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System.IO;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Файл файл Microstation
    /// </summary>
    public partial class DocumentMicrostation : IDocumentLibraryElements
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer StampContainer { get; }

        /// <summary>
        /// Найти таблицы-штампы во всех моделях и листах
        /// </summary>
        private IEnumerable<IStamp> FindStamps(IEnumerable<IModelMicrostation> modelsMicrostation) => 
            modelsMicrostation.SelectMany(model => model.FindStamps()).ToList();
    }
}
