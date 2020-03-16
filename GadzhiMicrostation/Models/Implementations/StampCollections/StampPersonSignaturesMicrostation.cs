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
    public class StampPersonSignaturesMicrostation<TElement, TElementSignature> : 
                        StampPersonSignature<IStampFieldMicrostation<TElement>, IStampFieldMicrostation<TElementSignature>>
                                             where TElement : class, ITextElementMicrostation
                                             where TElementSignature : class, ICellElementMicrostation
                        
    {
        public StampPersonSignaturesMicrostation(IEnumerable<ITextElementMicrostation> elementsMicrostation)
        {
            ActionType = GetFieldFromMicrostationElements(elementsMicrostation, StampFieldPersonSignatures.GetFieldsSignaturesActionType());
            ResponsiblePerson = GetFieldFromMicrostationElements(elementsMicrostation, StampFieldPersonSignatures.GetFieldsSignaturesResponsiblePerson());
            DateSignature = GetFieldFromMicrostationElements(elementsMicrostation, StampFieldPersonSignatures.GetFieldsSignaturesDateSignature());   
        }

        public StampPersonSignaturesMicrostation(IStampFieldMicrostation<TElement> actionType,
                                                 IStampFieldMicrostation<TElement> responsiblePerson,
                                                 IStampFieldMicrostation<TElementSignature> signature,
                                                 IStampFieldMicrostation<TElement> dateSignature)
        {
            ActionType = actionType;
            ResponsiblePerson = responsiblePerson;
            Signature = signature;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public override IStampFieldMicrostation<TElement> ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public override IStampFieldMicrostation<TElement> ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldMicrostation<TElementSignature> Signature { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldMicrostation<TElement> DateSignature { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string AttributePersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

        /// <summary>
        /// Получить поля с ответственным лицом и подписью из списка элементов Microstation
        /// </summary>
        private IStampFieldMicrostation<TElement> GetFieldFromMicrostationElements(IEnumerable<ITextElementMicrostation> elementsMicrostation, HashSet<StampFieldBase> stampFields) =>        
                elementsMicrostation?.Where(element => stampFields?.
                                                       Select(field => field.Name).
                                                       Contains(element.AttributeControlName) == true)?.
                                      Select(field => new StampFieldMicrostation<TElement>(field as TElement, StampFieldType.PersonSignature))?.
                                      FirstOrDefault();
      
    }
}
