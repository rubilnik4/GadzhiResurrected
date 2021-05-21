using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiWord.Word.Implementations.Converters
{
    /// <summary>
    /// Преобразование формата листа Word в строковое значение
    /// </summary>
    public static class WordPaperSizeToString
    {
        /// <summary>
        /// Преобразовать формат листа в строковое значение
        /// </summary>
        public static StampPaperSizeType PaperSizeToString(WdPaperSize paperSize) =>
            paperSize switch
            {
                WdPaperSize.wdPaperA3 => StampPaperSizeType.A3,
                WdPaperSize.wdPaperA4 => StampPaperSizeType.A4,
                _ => StampPaperSizeType.Undefined,
            };
    }
}
