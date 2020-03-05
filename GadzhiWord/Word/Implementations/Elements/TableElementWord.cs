using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.Elements
{
    /// <summary>
    /// Элемент таблица
    /// </summary>
    public class TableElementWord: ITableElementWord
    {
        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        private readonly Table _tableElement;

        public TableElementWord(Table tableElement)
        {
            _tableElement = tableElement;
        }
    }
}
