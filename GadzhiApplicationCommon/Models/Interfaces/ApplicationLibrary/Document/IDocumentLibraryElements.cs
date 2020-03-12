using GadzhiApplicationCommon.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document
{
    /// <summary>
    /// Подкласс документа Word для работы с элементами
    /// </summary>
    public interface IDocumentLibraryElements
    {
        /// <summary>
        /// Найти нижние колонтитулы
        /// </summary>
        IEnumerable<ITableElement> GetTablesInFooters();
    }
}
