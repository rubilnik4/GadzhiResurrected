using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Models.Implementations.StampCollections.Signatures;

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
        private IResultAppCollection<IStampPerson> GetStampPersonRows() =>
            GetStampSignatureRows(StampFieldType.PersonSignature, GetPersonSignatureField).
            Map(signatureRows => new ResultAppCollection<IStampPerson>(signatureRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                           "Штамп основных подписей не найден")));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IResultAppValue<IStampPerson> GetPersonSignatureField(IEnumerable<string> personNames)
        {
            var foundElements = FindElementsInStampControls(personNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>().
                                ToList();
            if (foundElements.Count == 0) return new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Поля основных подписей не найдены").
                                                 ToResultApplicationValue<IStampPerson>();

            var actionType = GetFieldFromElements(foundElements, StampFieldPersons.GetFieldsActionType(), StampFieldType.PersonSignature);
            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldPersons.GetFieldsResponsiblePerson(), StampFieldType.PersonSignature);
            var dateSignature = GetFieldFromElements(foundElements, StampFieldPersons.GetFieldsDateSignature(), StampFieldType.PersonSignature);
            var insertSignatureFunc = InsertSignatureFunc(responsiblePerson.ElementStamp, dateSignature.ElementStamp, StampFieldType.PersonSignature);

            return new StampPersonMicrostation(SignaturesLibrarySearching.FindById(), insertSignatureFunc, 
                                               actionType, responsiblePerson, dateSignature).
                   Map(stampPersonSignature => new ResultAppValue<IStampPerson>(stampPersonSignature));
        }
    }
}
