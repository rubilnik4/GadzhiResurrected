using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

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
        private IResultAppCollection<IStampPersonMicrostation> GetStampPersonRows() =>
            GetStampSignatureRows(StampFieldType.PersonSignature, GetPersonSignatureField).
            Map(signatureRows => new ResultAppCollection<IStampPersonMicrostation>(signatureRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                                       "Штамп основных подписей не найден")));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IResultAppValue<IStampPersonMicrostation> GetPersonSignatureField(IEnumerable<string> personNames)
        {
            var foundElements = FindElementsInStampControls(personNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>().
                                ToList();
            if (foundElements.Count == 0) return new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Поля основных подписей не найдены").
                                                 ToResultApplicationValue<IStampPersonMicrostation>();

            var actionType = GetFieldFromElements(foundElements, StampFieldPersons.GetFieldsActionType(), StampFieldType.PersonSignature);
            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldPersons.GetFieldsResponsiblePerson(), StampFieldType.PersonSignature);
            var dateSignature = GetFieldFromElements(foundElements, StampFieldPersons.GetFieldsDateSignature(), StampFieldType.PersonSignature);
            var insertSignatureFunc = InsertSignatureFunc(responsiblePerson.ElementStamp, dateSignature.ElementStamp, StampFieldType.PersonSignature);

            return new StampPersonMicrostation(actionType, responsiblePerson, dateSignature, insertSignatureFunc).
                   Map(stampPersonSignature => new ResultAppValue<IStampPersonMicrostation>(stampPersonSignature));
        }
    }
}
