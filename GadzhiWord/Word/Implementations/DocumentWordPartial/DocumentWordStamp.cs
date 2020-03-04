using GadzhiCommon.Extentions.StringAdditional;
using GadzhiWord.Models.StampCollections;
using GadzhiWord.Word.Implementations.StampPartial;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using GadzhiWord.Word.Interfaces.StampPartial;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы со штампом
    /// </summary>
    public partial class DocumentWord : IDocumentWordStamp
    {
        /// <summary>
        /// Найти штампы в документе
        /// </summary>    
        private IEnumerable<IStampWord> FindStamps() => GetTablesInFooters().Where(CheckFooterIsStamp).
                                                                             Select(table => new StampWord(table));


        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private bool CheckFooterIsStamp(Table table)
        {
            GetCells(table?.Range.Cells).Where(cell => !String.IsNullOrWhiteSpace(cell?.Range?.Text))
                                        .Select(cell => PrepareCellText(cell.Range.Text))
                                        .Where(cellText => StampAdditionalParameters.MarkersAdditionalStamp.Any());

            return false;
        }

        /// <summary>
        /// Обработать текст ячейки
        /// </summary>        
        private string PrepareCellText(string cellText)
        {
            string preparedText = cellText.Replace("ё", "е").
                                           Replace("c", "с").
                                           Replace("у", "у").
                                           Replace("o", "о").
                                           Replace("..", ".").
                                           Trim((char)10, (char)11, (char)13, (char)160, (char)176);

            preparedText = Regex.Replace(preparedText, @"\s+", " ");
            return preparedText;
        }
    }
}
