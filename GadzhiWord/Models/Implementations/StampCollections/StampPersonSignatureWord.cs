using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
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
    public class StampPersonSignatureWord : StampPersonSignature<IStampFieldWord>
    {       
        public StampPersonSignatureWord(IRowElement rowElementWord)
        {
            if (rowElementWord?.CellsElementWord?.Count >= 4)
            {               
                if (CheckFieldType.IsFieldPersonSignatureWithPrepare(rowElementWord?.CellsElementWord[0].Text))
                {
                    ActionType = new StampFieldWord(rowElementWord?.CellsElementWord[0], StampFieldType.PersonSignature);
                    ResponsiblePerson = new StampFieldWord(rowElementWord?.CellsElementWord[1], StampFieldType.PersonSignature);
                    Signature = new StampFieldWord(rowElementWord?.CellsElementWord[2], StampFieldType.PersonSignature);
                    DateSignature = new StampFieldWord(rowElementWord?.CellsElementWord[3], StampFieldType.PersonSignature);
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
        public override IStampFieldWord ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public override IStampFieldWord ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldWord Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldWord DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary> 
        public override string AttributePersonId => throw new NotImplementedException();

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature?.CellElementStamp?.HasPicture == true;
    }
}
