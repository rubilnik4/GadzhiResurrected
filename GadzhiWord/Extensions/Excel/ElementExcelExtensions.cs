using System.Collections.Generic;
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
    }
}