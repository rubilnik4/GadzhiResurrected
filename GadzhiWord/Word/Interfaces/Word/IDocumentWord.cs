using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Word.Interfaces.Word
{
    /// <summary>
    /// Документ приложения Word
    /// </summary>
    public interface IDocumentWord : IDocumentLibrary
    {
        /// <summary>
        /// Класс для работы с приложением Word
        /// </summary>
        IApplicationOffice ApplicationOffice { get; }

        /// <summary>
        /// Таблицы
        /// </summary>
        public IReadOnlyList<ITableElementWord> Tables { get; }

        /// <summary>
        /// Экспорт в DocX
        /// </summary>
        string ExportToDocx();
    }
}
