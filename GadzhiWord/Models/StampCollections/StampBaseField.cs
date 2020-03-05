using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>
    public class StampBaseField
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        private readonly ICellElementWord _cellElementWord;

        public StampBaseField(ICellElementWord cellElementWord)
        {
            _cellElementWord = cellElementWord;
        }       
    }
}
