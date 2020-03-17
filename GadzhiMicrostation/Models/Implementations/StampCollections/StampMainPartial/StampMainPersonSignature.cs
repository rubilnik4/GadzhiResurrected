using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Строки с ответственными лицами в основном штампе
    /// </summary>
    public partial class StampMainMicrostation
    {
        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IEnumerable<IStampPersonSignatureMicrostation> GetStampPersonRowsWithoutSignatures() =>
            StampFieldPersonSignatures.GetStampRowPersonSignatures().
                                       Select(signatureRow => signatureRow.StampPersonSignatureFields.Select(field => field.Name)).
                                       Select(signatureName => GetPersonSignatureField(signatureName));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IStampPersonSignatureMicrostation GetPersonSignatureField(IEnumerable<string> signatureNames)
        {
            var foundElements = FindElementsInStampFields(signatureNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>();

            var actionType = GetFieldFromElements(foundElements, StampFieldPersonSignatures.GetFieldsActionType(),
                                                  StampFieldType.PersonSignature);

            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldPersonSignatures.GetFieldsResponsiblePerson(),
                                                         StampFieldType.PersonSignature);

            var dateSignature = GetFieldFromElements(foundElements, StampFieldPersonSignatures.GetFieldsDateSignature(),
                                                     StampFieldType.PersonSignature);

            return new StampPersonSignatureMicrostation(actionType, responsiblePerson, dateSignature);
        }

        /// <summary>
        /// Получить строку с ответственным лицом с подписью
        /// </summary>
        private IStampPersonSignatureMicrostation GetStampPersonRowWithSignatures(IStampPersonSignatureMicrostation person) =>
            new StampPersonSignatureMicrostation(person.ActionType,
                                                 person.ResponsiblePerson,
                                                 new StampFieldMicrostation(InsertPersonSignatureFromLibrary(person),
                                                                            StampFieldType.PersonSignature),
                                                 person.DateSignature)
            as IStampPersonSignatureMicrostation;

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private ICellElementMicrostation InsertPersonSignatureFromLibrary(IStampPersonSignatureMicrostation personSignature) =>
           InsertSignature(personSignature.AttributePersonId,
                           personSignature.ResponsiblePersonElement.Text,
                           personSignature.ResponsiblePersonElement,
                           personSignature.DateSignatureElement);
    }
}
