using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Interfaces.Elements;
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
        public StampPersonSignature(IRowElement rowElementWord)
        {
            if (rowElementWord?.CellsElementWord?.Count >= 4)
            {               
                if (CheckFieldType.IsFieldPersonSignatureWithPrepare(rowElementWord?.CellsElementWord[0].Text))
                {
                    ActionType = new StampField(rowElementWord?.CellsElementWord[0]);
                    ResponsiblePerson = new StampField(rowElementWord?.CellsElementWord[1]);
                    Signature = new StampField(rowElementWord?.CellsElementWord[2]);
                    DateSignature = new StampField(rowElementWord?.CellsElementWord[3]);
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
