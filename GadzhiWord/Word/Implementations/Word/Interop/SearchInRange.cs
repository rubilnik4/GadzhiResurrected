using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiWord.Extensions.Word;
using Microsoft.Office.Interop.Word;

namespace GadzhiWord.Word.Implementations.Word.Interop
{
    /// <summary>
    /// Поиск в диапазоне элементов
    /// </summary>
    public static class SearchInRange
    {
        /// <summary>
        /// Найти таблицы в колонтитуле
        /// </summary>
        public static IEnumerable<Table> GetTablesFromFooter(HeaderFooter footer) =>
            footer.Range.Tables.ToEnumerable().
            Concat(GetTablesFromShapes(footer.Shapes));

        /// <summary>
        /// Найти таблицы в фигуре
        /// </summary>
        public static IEnumerable<Table> GetTablesFromShapes(Shapes shapes) =>
            shapes.ToEnumerable().SelectMany(shape => shape.TextFrame.ContainingRange.Tables.ToEnumerable());
    }
}
