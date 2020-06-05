using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Extensions.Word
{
    /// <summary>
    /// Расширения для элементов Word
    /// </summary>
    public static class ElementWordExtensions
    {
        /// <summary>
        /// Получить номер страницы элемента
        /// </summary>
        public static int GetPageNumber(this Range range) => range.Information[WdInformation.wdActiveEndPageNumber];

        /// <summary>
        /// Получить разделы
        /// </summary>        
        public static IEnumerable<Section> ToEnumerable(this Sections sections)
        {
            if (sections == null) yield break;

            foreach (Section section in sections)
            {
                yield return section;
            }
        }

        /// <summary>
        /// Получить список нижних колонтитулов
        /// </summary>       
        public static IEnumerable<HeaderFooter> ToEnumerable(this HeadersFooters headersFooters)
        {
            if (headersFooters == null) yield break;

            foreach (HeaderFooter headerFooters in headersFooters)
            {
                yield return headerFooters;
            }
        }

        /// <summary>
        /// Получить список фигур
        /// </summary>       
        public static IEnumerable<Shape> ToEnumerable(this Shapes shapes)
        {
            if (shapes == null) yield break;

            foreach (Shape shape in shapes)
            {
                yield return shape;
            }
        }

        /// <summary>
        /// Получить таблицы
        /// </summary>        
        public static IEnumerable<Table> ToEnumerable(this Tables tables)
        {
            if (tables == null) yield break;

            foreach (Table table in tables)
            {
                yield return table;
            }
        }

        /// <summary>
        /// Получить строки
        /// </summary>        
        public static IEnumerable<Row> ToEnumerable(this Rows rows)
        {
            if (rows == null) yield break;

            foreach (Row row in rows)
            {
                yield return row;
            }
        }

        /// <summary>
        /// Получить ячейки
        /// </summary>        
        public static IEnumerable<Cell> ToEnumerable(this Cells cells)
        {
            if (cells == null) yield break;

            foreach (Cell cell in cells)
            {
                yield return cell;
            }
        }
    }
}
