using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Models.Implementations.StampCollections.Signatures;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Строки с ответственными лицами в основном штампе
    /// </summary>
    public partial class StampMainMicrostation
    {
        /// <summary>
        /// Получить строки основных подписей с ответственным лицом без подписи
        /// </summary>
        private IResultAppCollection<IStampPerson> GetStampPersonRows() =>
            GetStampSignatureRows(StampFieldType.PersonSignature, GetStampPersonRow).
            Map(signatureRows => new ResultAppCollection<IStampPerson>(signatureRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                           "Штамп основных подписей не найден")));

        /// <summary>
        /// Преобразовать элементы Microstation в строку основных подписей
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonRow(IEnumerable<string> personNames) =>
            FindElementsInStamp<ITextElementMicrostation>(personNames, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                            "Поля основных подписей не найдены")).
            ResultValueOkBind(GetStampPersonFromFields);

        /// <summary>
        /// Получить строку с основной подписью из полей штампа
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonFromFields(IList<ITextElementMicrostation> foundFields)
        {
            var actionType = GetFieldFromElements(foundFields, StampFieldPersons.GetFieldsActionType(), StampFieldType.PersonSignature);
            var responsiblePerson = GetFieldFromElements(foundFields, StampFieldPersons.GetFieldsResponsiblePerson(), StampFieldType.PersonSignature);
            var dateSignature = GetFieldFromElements(foundFields, StampFieldPersons.GetFieldsDateSignature(), StampFieldType.PersonSignature);
            var insertSignatureFunc = InsertSignatureFunc(responsiblePerson.ElementStamp, dateSignature.ElementStamp, StampFieldType.PersonSignature);

            return GetStampPersonById(responsiblePerson, insertSignatureFunc, actionType, dateSignature);
        }

        /// <summary>
        /// Сформировать строку с основной подписью согласно идентификатору
        /// </summary>
        private IResultAppValue<IStampPerson> GetStampPersonById(IStampTextFieldMicrostation responsiblePerson,
                                                                 Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                                                 IStampTextField actionType, IStampTextField dateSignature) =>
            SignaturesSearching.FindByIdOrFullNameOrRandom(responsiblePerson.ElementStamp.AttributePersonId, 
                                                           responsiblePerson.Text, StampSettings.PersonId).
            ResultValueOk(personSignature => new StampPersonMicrostation(personSignature, insertSignatureFunc, actionType, 
                                                                         responsiblePerson, dateSignature));
    }
}
