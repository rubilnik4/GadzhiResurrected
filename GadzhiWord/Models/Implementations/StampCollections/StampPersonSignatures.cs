using GadzhiWord.Models.Interfaces.StampCollections;
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
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public class StampPersonSignature: IStampPersonSignature
    {
        /// <summary>
        /// Элемент строка
        /// </summary>
        private readonly IRowElementWord _rowElementWord;        

        public StampPersonSignature(IRowElementWord rowElementWord)
        {
            if (rowElementWord?.CellsElementWord?.Count >= 4)
            {
                if (CheckFieldType.IsFieldPersonSignature(rowElementWord?.CellsElementWord[0].Text))
                {
                    ActionType = new StampField(rowElementWord?.CellsElementWord[0]);
                    ResponsiblePerson = new StampField(rowElementWord?.CellsElementWord[1]);
                    Signature = new StampField(rowElementWord?.CellsElementWord[2]);
                    DateSignature = new StampField(rowElementWord?.CellsElementWord[3]);

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
        public IStampField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampField Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampField DateSignature { get; }     
    }
}
