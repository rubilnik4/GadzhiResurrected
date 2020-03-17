using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Microstation
    /// </summary>
    public class StampPersonSignatureMicrostation : StampPersonSignature<IStampFieldMicrostation>,
                                                    IStampPersonSignatureMicrostation
    {
        public StampPersonSignatureMicrostation(IStampFieldMicrostation actionType, IStampFieldMicrostation responsiblePerson,
                                                IStampFieldMicrostation dateSignature)
            : this(actionType, responsiblePerson, null, dateSignature) { }

        public StampPersonSignatureMicrostation(IStampPersonSignature<IStampFieldMicrostation> personSignature)
            : this(personSignature?.ActionType, personSignature?.ResponsiblePerson,
                  personSignature?.Signature, personSignature?.DateSignature)
        {
            if (personSignature == null)
                throw new ArgumentNullException(nameof(personSignature));
        }

        public StampPersonSignatureMicrostation(IStampFieldMicrostation actionType, IStampFieldMicrostation responsiblePerson,
                                                IStampFieldMicrostation signature, IStampFieldMicrostation dateSignature)
        {
            ActionType = actionType;
            ResponsiblePerson = responsiblePerson;
            Signature = signature;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public override IStampFieldMicrostation ActionType { get; }

        /// <summary>
        /// Тип действия. Элемент
        /// </summary>
        public ITextElementMicrostation ActionTypeElement => ActionType.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public override IStampFieldMicrostation ResponsiblePerson { get; }

        /// <summary>
        /// Ответственное лицо. Элемент
        /// </summary>
        public ITextElementMicrostation ResponsiblePersonElement => ResponsiblePerson.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Подпись
        /// </summary>
        public override IStampFieldMicrostation Signature { get; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        public ICellElementMicrostation SignatureElement => Signature.ElementStamp.AsCellElementMicrostation;

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldMicrostation DateSignature { get; }

        /// <summary>
        /// Дата. Элемент
        /// </summary>
        public ITextElementMicrostation DateSignatureElement => DateSignature.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string AttributePersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

    }
}
