using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
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
    public class StampPersonSignatureMicrostation : StampSignatureMicrostation,
                                                    IStampPersonSignatureMicrostation
    {       
        public StampPersonSignatureMicrostation(IStampFieldMicrostation actionType, IStampFieldMicrostation responsiblePerson,
                                                IStampFieldMicrostation dateSignature,
                                                Func<string, IStampFieldMicrostation> insertSignatureFunc)
            : base(insertSignatureFunc)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            ActionType = actionType;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public IStampFieldMicrostation ActionType { get; }

        /// <summary>
        /// Тип действия. Элемент
        /// </summary>
        public ITextElementMicrostation ActionTypeElement => ActionType.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampFieldMicrostation ResponsiblePerson { get; }

        /// <summary>
        /// Ответственное лицо. Элемент
        /// </summary>
        public ITextElementMicrostation ResponsiblePersonElement => ResponsiblePerson.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Дата
        /// </summary>
        public IStampFieldMicrostation DateSignature { get; }

        /// <summary>
        /// Дата. Элемент
        /// </summary>
        public ITextElementMicrostation DateSignatureElement => DateSignature.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string PersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName => ResponsiblePersonElement.Text;
    }
}
