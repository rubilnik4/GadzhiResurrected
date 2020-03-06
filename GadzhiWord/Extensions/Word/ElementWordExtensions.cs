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
        public static int GetPageNumber(this Range range) => range?.Information[WdInformation.wdActiveEndPageNumber];

        /// <summary>
        /// Получить разделы
        /// </summary>        
        public static IEnumerable<Section> ToIEnumerable(this Sections sections)
        {
            if (sections != null)
            {
                foreach (Section section in sections)
                {
                    yield return section;
                }
            }
            else
            {
                yield return null;
            }
        }

        /// <summary>
        /// Получить список нижних колонтитулов
        /// </summary>       
        public static IEnumerable<HeaderFooter> ToIEnumerable(this HeadersFooters headersFooters)
        {
            if (headersFooters != null)
            {
                foreach (HeaderFooter headerFooters in headersFooters)
                {
                    yield return headerFooters;
                }
            }
            else
            {
                yield return null;
            }
        }

        /// <summary>
        /// Получить таблицы
        /// </summary>        
        public static IEnumerable<Table> ToIEnumerable(this Tables tables)
        {
            if (tables != null)
            {
                foreach (Table table in tables)
                {
                    yield return table;
                }
            }
            else
            {
                yield return null;
            }
        }

        /// <summary>
        /// Получить строки
        /// </summary>        
        public static IEnumerable<Row> ToIEnumerable(this Rows rows)
        {
            if (rows != null)
            {
                foreach (Row row in rows)
                {
                    yield return row;
                }
            }
            else
            {
                yield return null;
            }
        }

        /// <summary>
        /// Получить ячейки
        /// </summary>        
        public static IEnumerable<Cell> ToIEnumerable(this Cells cells)
        {
            if (cells != null)
            {
                foreach (Cell cell in cells)
                {
                    yield return cell;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}
