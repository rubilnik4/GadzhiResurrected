using GadzhiWord.Word.Interfaces.StampPartial;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.StampPartial
{
    /// <summary>
    /// Штамп
    /// </summary>
    public class StampWord : IStampWord
    {
        /// <summary>
        /// Элемент таблица опредеяющая штамп
        /// </summary>
        private readonly Table _tableStamp;

        public StampWord(Table tableStamp)
        {
            _tableStamp = tableStamp;
        }

    }
}
