using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Строки с изменениями в основном штампе
    /// </summary>
    public partial class StampMainMicrostation
    {
        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        private IResultAppCollection<IStampChangeMicrostation> GetStampChangeRows(ISignatureLibrary signatureLibrary) =>
            new ResultAppValue<ISignatureLibrary>(signatureLibrary, new ErrorApplication(ErrorApplicationType.SignatureNotFound, 
                                                                                         "Не найден идентификатор основной подписи")).
            ResultValueOk(signature => GetStampSignatureRows(StampFieldType.ChangeSignature, 
                                                             changeNames => GetChangeSignatureField(changeNames, signatureLibrary))).
            ToResultCollection(new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Штамп подписей замены не найден"));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IResultAppValue<IStampChangeMicrostation> GetChangeSignatureField(IEnumerable<string> changeNames, ISignatureLibrary signatureLibrary)
        {
            var foundElements = FindElementsInStampControls(changeNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>().
                                ToList();
            if (foundElements.Count == 0) return new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Поля подписей замены не найдены").
                                                 ToResultApplicationValue<IStampChangeMicrostation>();

            var numberChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsNumberChange(), StampFieldType.ChangeSignature);
            var numberOfPlots = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsNumberOfPlots(), StampFieldType.ChangeSignature);
            var typeOfChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsTypeOfChange(), StampFieldType.ChangeSignature);
            var documentChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsDocumentChange(), StampFieldType.ChangeSignature);
            var dateChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsDateChange(), StampFieldType.ChangeSignature);
            var insertSignatureFunc = InsertSignatureFunc(documentChange.ElementStamp, dateChange.ElementStamp, StampFieldType.ChangeSignature);

            return new StampChangeMicrostation(numberChange, numberOfPlots, typeOfChange, documentChange,
                                               dateChange, signatureLibrary, insertSignatureFunc).
                   Map(stampChangeSignature => new ResultAppValue<IStampChangeMicrostation>(stampChangeSignature));
        }
    }
}
