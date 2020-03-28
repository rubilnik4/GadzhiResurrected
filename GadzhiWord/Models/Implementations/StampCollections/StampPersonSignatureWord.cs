using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
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
    public class StampPersonSignatureWord : StampSignatureWord, IStampPersonSignatureWord
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public static int FieldsCount => 4;
       
        public StampPersonSignatureWord(IStampFieldWord actionType, IStampFieldWord responsiblePerson,
                                        IStampFieldWord signature, IStampFieldWord dateSignature,
                                        ISignatureInformation signatureInformation)
            : base(signature, signatureInformation?.SignaturePath)
        {
            PersonId = signatureInformation?.PersonId ?? throw new ArgumentNullException(nameof(signatureInformation));
            PersonName = signatureInformation?.PersonName;

            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            ActionType = actionType;
            DateSignature = dateSignature;      
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public IStampFieldWord ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampFieldWord ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampFieldWord DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName { get; }
    }
}
