using System;
using System.Collections.Generic;
using GadzhiWord.Word.Implementations.Excel.Helpers;
using Microsoft.Office.Interop.Excel;

namespace GadzhiWord.Extensions.Excel
{
    /// <summary>
    /// Расширения для элементов Excel
    /// </summary>
    public static class ElementExcelExtensions
    {
        /// <summary>
        /// Получить листы
        /// </summary>        
        public static IEnumerable<Worksheet> ToIEnumerable(this Sheets worksheets)
        {
            if (worksheets == null) yield break;

            foreach (Worksheet worksheet in worksheets)
            {
                yield return worksheet;
            }
        }

        /// <summary>
        /// Получить колонку по индексу
        /// </summary>
        public static Range GetColumnByIndex(this Range columns, int index) =>
            (columns != null)
            ? columns[ColumnNamesExcel.GetExcelColumnName(index), Type.Missing]
            : throw new ArgumentNullException(nameof(columns));
        
    }
}