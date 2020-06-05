using System;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampCreating
{
    /// <summary>
    /// Таблица Word и соответствующее значение типа штампа
    /// </summary>
    public class TableStampType
    {
        public TableStampType(StampType stampType, ITableElementWord tableWord)
        {
            StampType = stampType;
            TableWord = tableWord ?? throw new ArgumentNullException(nameof(tableWord));
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public StampType StampType { get; }

        /// <summary>
        /// Таблица
        /// </summary>
        public ITableElementWord TableWord { get; }
    }
}