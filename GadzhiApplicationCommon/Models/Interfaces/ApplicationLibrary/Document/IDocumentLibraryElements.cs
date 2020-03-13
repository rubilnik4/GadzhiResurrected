using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document
{
    /// <summary>
    /// Подкласс документа для работы с элементами
    /// </summary>
    public interface IDocumentLibraryElements
    {
        /// <summary>
        /// Найти таблицы в документе
        /// </summary>
        IEnumerable<IStamp> FindStamps();
    }
}
