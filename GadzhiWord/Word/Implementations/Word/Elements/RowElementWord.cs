using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Word.Implementations.Word.Elements
{
    /// <summary>
    /// Элемент строка
    /// </summary>
    public class RowElementWord : IRowElementWord
    {
        public RowElementWord(IEnumerable<ICellElementWord> cellsElement)
        {
            var cellsElementCollection = cellsElement?.ToList();
            CellsElement = ValidateCellsRow(cellsElementCollection) 
                            ? cellsElementCollection 
                            : throw new ArgumentNullException(nameof(cellsElement));
        }

        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        public IReadOnlyList<ICellElementWord> CellsElement { get; }

        /// <summary>
        /// Индекс строки
        /// </summary>
        public int Index => CellsElement[0].RowIndex;

        /// <summary>
        /// Проверить корректность ячеек строки
        /// </summary>
        public static bool ValidateCellsRow(IList<ICellElementWord> cells) => cells?.Count > 0;
    }
}
