using GadzhiCommon.Extentions.StringAdditional;
using GadzhiWord.Extension;
using GadzhiWord.Models.Enums;
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
        private bool CheckFooterIsStamp(Table table) =>      
            GetCells(table?.Range.Cells).Where(cell => !String.IsNullOrWhiteSpace(cell?.Range?.Text)).
                                         Select(cell => StringExtensions.PrepareCellTextToComprare(cell.Range.Text)).
                                         Any(cellText => StampAdditionalParameters.MarkersStamp.
                                                           Any(marker => cellText.Contains(marker)));      

    } 
}
