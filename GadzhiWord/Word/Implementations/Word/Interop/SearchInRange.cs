using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Extensions.Word;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Shapes = Microsoft.Office.Interop.Word.Shapes;

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
            footer.Exists
                ? footer.Range.Tables.ToEnumerable().
                  ToList().
                  WhereBad(tablesFooters => tablesFooters.Count > 0,
                    badFunc: _ => GetTablesFromShapes(footer.Shapes).ToList())
                : Enumerable.Empty<Table>();

        /// <summary>
        /// Найти таблицы в фигуре
        /// </summary>
        public static IEnumerable<Table> GetTablesFromShapes(Shapes shapes) =>
            shapes.ToEnumerable().
            Where(shape => shape.AutoShapeType != MsoAutoShapeType.msoShapeMixed).
            SelectMany(shape => shape.TextFrame.ContainingRange.Tables.ToEnumerable());
    }
}
