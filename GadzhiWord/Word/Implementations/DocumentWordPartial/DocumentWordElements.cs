using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы с элементами
    /// </summary>
    public partial class DocumentWord : IDocumentWordElements
    {
        /// <summary>
        /// Найти нижние колонтитулы
        /// </summary>
        private IEnumerable<Table> GetTablesInFooters() =>      
                GetSections().SelectMany(section => GetFooters(section.Footers))
                             .SelectMany(footer => GetTables(footer.Range.Tables));          
       

        /// <summary>
        /// Получить разделы
        /// </summary>        
        private IEnumerable<Section> GetSections()
        {
            foreach (Section section in _document.Sections)
            {
                yield return section;
            }
        }      

        /// <summary>
        /// Получить список нижних колонтитулов
        /// </summary>       
        private IEnumerable<HeaderFooter> GetFooters(HeadersFooters headersFooters)
        {
            foreach (HeaderFooter headerFooters in headersFooters)
            {
                yield return headerFooters;
            }
        }

        /// <summary>
        /// Получить таблицы
        /// </summary>        
        private IEnumerable<Table> GetTables(Tables tables)
        {
            foreach (Table table in tables)
            {
                yield return table;
            }
        }

        /// <summary>
        /// Получить ячейки
        /// </summary>        
        private IEnumerable<Cell> GetCells(Cells cells)
        {
            foreach (Cell cell in cells)
            {
                yield return cell;
            }
        }
    }
}
