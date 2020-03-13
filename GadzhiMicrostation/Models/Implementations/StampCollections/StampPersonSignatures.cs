using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с ответсвенным лицом и подписью
    /// </summary>
    public class StampPersonSignature : IStampPersonSignature
    {
        public StampPersonSignature(IEnumerable<IElementMicrostation> elementsMicrostation)
        {
            ActionType = elementsMicrostation.Where(element => StampFieldPersonSignatures.GetFieldsSignaturesActionType().
                                                                             Select(field => field.Name).
                                                                             Contains(element.AttributeControlName))?.
                                              Select(field => new StampField(field, StampFieldType.PersonSignature))?.
                                              FirstOrDefault();

            ResponsiblePerson = elementsMicrostation.Where(element => StampFieldPersonSignatures.GetFieldsSignaturesResponsiblePerson().
                                                                             Select(field => field.Name).
                                                                             Contains(element.AttributeControlName))?.
                                              Select(field => new StampField(field, StampFieldType.PersonSignature))?.
                                              FirstOrDefault();

            DateSignature = elementsMicrostation.Where(element => StampFieldPersonSignatures.GetFieldsSignaturesDateSignature().
                                                                             Select(field => field.Name).
                                                                             Contains(element.AttributeControlName))?.
                                              Select(field => new StampField(field, StampFieldType.PersonSignature))?.
                                              FirstOrDefault();
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
