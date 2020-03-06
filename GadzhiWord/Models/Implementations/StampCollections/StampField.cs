using GadzhiWord.Models.Enums;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Implementations.Elements;
using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>
    public class StampField : IStampField
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        public ICellElementWord CellElementWord { get; }

        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        public StampFieldType StampFieldType { get; }

        public StampField(ICellElementWord cellElementWord)
        {
            CellElementWord = cellElementWord;
            StampFieldType = CheckFieldType.GetStampFieldType(cellElementWord?.Text);
        }

        /// <summary>
        /// Родительский элемент строка
        /// </summary>
        public IRowElementWord RowElementStamp => CellElementWord.RowElementWord;
    }
}
