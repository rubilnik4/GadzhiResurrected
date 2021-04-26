using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
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
    /// Строки с изменениями в основном штампе Microstation
    /// </summary>
    public partial class SignatureCreatingMicrostation
    {
        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        public override IResultAppCollection<IStampChange> GetStampChangeRows(ISignatureLibraryApp signatureLibrary) =>
            new ResultAppValue<ISignatureLibraryApp>(signatureLibrary, new ErrorApplication(ErrorApplicationType.SignatureNotFound, 
                                                                                            "Не найден идентификатор основной подписи")).
            ResultValueOk(signature => GetStampSignatureRows(StampFieldType.ChangeSignature, 
                                                             changeNames => GetStampChangeRow(changeNames, signatureLibrary))).
            ResultValueOk(changeRows => changeRows.Where(changeRow => !String.IsNullOrEmpty(changeRow.DocumentChange.Text))).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп подписей замены не найден"));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей изменений Microstation
        /// </summary>
        private IResultAppValue<IStampChange> GetStampChangeRow(IEnumerable<string> changeNames, ISignatureLibraryApp personSignature) =>
             _stampFields.FindElementsInStamp<ITextElementMicrostation>(changeNames, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                                      "Поля подписей изменений не найдены")).
            ResultValueOkBind(foundFields => GetStampChangeFromFields(foundFields, personSignature));

        /// <summary>
        /// Получить строку с подписью изменений из полей штампа Microstation
        /// </summary>
        private IResultAppValue<IStampChange> GetStampChangeFromFields(IList<ITextElementMicrostation> foundFields, ISignatureLibraryApp personSignature)
        {
            var numberChange = _stampFields.GetFieldFromElements(foundFields, StampFieldChanges.GetFieldsNumberChange(), StampFieldType.ChangeSignature);
            var numberOfPlots = _stampFields.GetFieldFromElements(foundFields, StampFieldChanges.GetFieldsNumberOfPlots(), StampFieldType.ChangeSignature);
            var typeOfChange = _stampFields.GetFieldFromElements(foundFields, StampFieldChanges.GetFieldsTypeOfChange(), StampFieldType.ChangeSignature);
            var documentChange = _stampFields.GetFieldFromElements(foundFields, StampFieldChanges.GetFieldsDocumentChange(), StampFieldType.ChangeSignature);
            var dateChange = _stampFields.GetFieldFromElements(foundFields, StampFieldChanges.GetFieldsDateChange(), StampFieldType.ChangeSignature);
            var insertSignatureFunc = InsertSignatureFunc(documentChange.ElementStamp, dateChange.ElementStamp, StampFieldType.ChangeSignature);

            return GetStampChangeById(personSignature, _stampIdentifier, insertSignatureFunc, numberChange,
                                      numberOfPlots, typeOfChange, documentChange, dateChange);
        }

        /// <summary>
        /// Сформировать строку с подписью изменений согласно идентификатору Microstation
        /// </summary>
        private static IResultAppValue<IStampChange> GetStampChangeById(ISignatureLibraryApp personSignature, StampIdentifier stampIdentifier,
                                                                 Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                                                 IStampTextField numberChange, IStampTextField numberOfPlots, IStampTextField typeOfChange,
                                                                 IStampTextField documentChange, IStampTextField dateChange) => 
            new ChangeSignatureMicrostation(personSignature, stampIdentifier, insertSignatureFunc, numberChange, numberOfPlots, 
                                            typeOfChange, documentChange, dateChange).
            Map(stampChange => new ResultAppValue<IStampChange>(stampChange));
    }
}
