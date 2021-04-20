using System;
using System.Collections.Generic;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.Signatures;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Строки с ответственными лицами в основном штампе Microstation
    /// </summary>
    public partial class SignatureCreatingMicrostation
    {
        /// <summary>
        /// Получить строки основных подписей с ответственным лицом без подписи Microstation
        /// </summary>
        public override IResultAppCollection<IStampPerson> GetStampPersonRows() =>
           GetStampSignatureRows(StampFieldType.PersonSignature, GetStampPersonRow).
           Map(signatureRows => new ResultAppCollection<IStampPerson>(signatureRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound, 
                                                                                                          "Штамп основных подписей не найден")));

        /// <summary>
        /// Преобразовать элементы Microstation в строку основных подписей Microstation
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonRow(IEnumerable<string> personNames) =>
            _stampFields.FindElementsInStamp<ITextElementMicrostation>(personNames, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                                     "Поля основных подписей не найдены")).
            ResultValueOkBind(GetStampPersonFromFields);

        /// <summary>
        /// Получить строку с основной подписью из полей штампа Microstation
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonFromFields(IList<ITextElementMicrostation> foundFields)
        {
            var actionType = _stampFields.GetFieldFromElements(foundFields, StampFieldPersons.GetFieldsActionType(), StampFieldType.PersonSignature);
            var responsiblePerson = _stampFields.GetFieldFromElements(foundFields, StampFieldPersons.GetFieldsResponsiblePerson(), StampFieldType.PersonSignature);
            var dateSignature = _stampFields.GetFieldFromElements(foundFields, StampFieldPersons.GetFieldsDateSignature(), StampFieldType.PersonSignature);
            var insertSignatureFunc = InsertSignatureFunc(responsiblePerson.ElementStamp, dateSignature.ElementStamp, StampFieldType.PersonSignature);

            return GetStampPersonById(responsiblePerson, insertSignatureFunc, actionType, dateSignature);
        }

        /// <summary>
        /// Сформировать строку с основной подписью согласно идентификатору Microstation
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonById(IStampTextFieldMicrostation responsiblePerson,
                                                                 Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                                                 IStampTextField actionType, IStampTextField dateSignature) =>
            SignaturesSearching.FindByIdOrFullNameOrRandom(responsiblePerson.ElementStamp.AttributePersonId,
                                                           responsiblePerson.MaxLengthWord, PersonId).
            ResultValueOk(personSignature => new PersonSignatureMicrostation(personSignature, _stampIdentifier, insertSignatureFunc, actionType,
                                                                         responsiblePerson, dateSignature));
    }
}
