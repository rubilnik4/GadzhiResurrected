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
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public class StampPersonSignature
    {
        /// <summary>
        /// Элемент строка
        /// </summary>
        private readonly IRowElementWord _rowElementWord;

        public StampPersonSignature(IRowElementWord rowElementWord)
        {
            if (rowElementWord?.CellsElementWord?.Count >= 4)
            {
                if (CheckFieldType.IsFieldPersonSignature(rowElementWord?.CellsElementWord[1].Text))
                {
                    ActionType = new StampBaseField(rowElementWord?.CellsElementWord[1]);
                    ResponsiblePerson = new StampBaseField(rowElementWord?.CellsElementWord[2]);
                    Signature = new StampBaseField(rowElementWord?.CellsElementWord[3]);
                    Date = new StampBaseField(rowElementWord?.CellsElementWord[4]);

                    _rowElementWord = rowElementWord;
                }
                else
                {
                    throw new ArgumentException(nameof(rowElementWord));
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(rowElementWord));
            }
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public StampBaseField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public StampBaseField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public StampBaseField Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public StampBaseField Date { get; }

       
    }
}
