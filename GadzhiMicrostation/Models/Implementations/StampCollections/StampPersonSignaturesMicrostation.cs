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
    public class StampPersonSignaturesMicrostation : StampPersonSignature<IStampFieldMicrostation>
    {
        public StampPersonSignaturesMicrostation(IEnumerable<IElementMicrostation> elementsMicrostation)
        {
            ActionType = elementsMicrostation.Where(element => StampFieldPersonSignatures.GetFieldsSignaturesActionType().
                                                                             Select(field => field.Name).
                                                                             Contains(element.AttributeControlName))?.
                                              Select(field => new StampFieldMicrostation(field, StampFieldType.PersonSignature))?.
                                              FirstOrDefault();

            ResponsiblePerson = elementsMicrostation.Where(element => StampFieldPersonSignatures.GetFieldsSignaturesResponsiblePerson().
                                                                             Select(field => field.Name).
                                                                             Contains(element.AttributeControlName))?.
                                                     Select(field => new StampFieldMicrostation(field, StampFieldType.PersonSignature))?.
                                                     FirstOrDefault();

            DateSignature = elementsMicrostation.Where(element => StampFieldPersonSignatures.GetFieldsSignaturesDateSignature().
                                                                             Select(field => field.Name).
                                                                             Contains(element.AttributeControlName))?.
                                                 Select(field => new StampFieldMicrostation(field, StampFieldType.PersonSignature))?.
                                                 FirstOrDefault();
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public override IStampFieldMicrostation ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public override IStampFieldMicrostation ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldMicrostation Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldMicrostation DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string AttributePersonId => ResponsiblePerson.TextElementStamp.AttributePersonId;
    }
}
