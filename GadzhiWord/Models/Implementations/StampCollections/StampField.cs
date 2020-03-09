using ConvertingModels.Models.Enums;
using ConvertingModels.Models.Interfaces.StampCollections;
using GadzhiConverting.Word.Interfaces.Elements;
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
        public ICellElement CellElementWord { get; }

        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        public StampFieldType StampFieldType { get; }

        public StampField(ICellElement cellElementWord)
        {
            CellElementWord = cellElementWord;
            StampFieldType = CheckFieldType.GetStampFieldType(cellElementWord?.Text);
        }

        /// <summary>
        /// Родительский элемент строка
        /// </summary>
        public IRowElement RowElementStamp => CellElementWord.RowElement;
    }
}
