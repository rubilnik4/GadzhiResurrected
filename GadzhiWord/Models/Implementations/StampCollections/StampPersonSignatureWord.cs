using GadzhiWord.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью
    /// </summary>
    public class StampPersonWord : StampSignatureWord, IStampPersonWord
    {
        /// <summary>
        /// Количество ячеек в строке
        /// </summary>
        public const int FIELDS_COUNT = 4;
       
        public StampPersonWord(IStampFieldWord actionType, IStampFieldWord responsiblePerson,
                                        IStampFieldWord signature, IStampFieldWord dateSignature,
                                        ISignatureInformation signatureInformation)
            : base(signature, signatureInformation?.SignaturePath)
        {
            PersonId = signatureInformation?.PersonId ?? throw new ArgumentNullException(nameof(signatureInformation));
            PersonName = signatureInformation.PersonName;

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
        /// идентификатор личности
        /// </summary>    
        public override string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName { get; }
    }
}
