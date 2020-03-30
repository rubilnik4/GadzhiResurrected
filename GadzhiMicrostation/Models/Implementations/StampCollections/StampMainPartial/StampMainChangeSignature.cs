using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiMicrostation.Extentions.StringAdditional;
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
    /// Строки с изменениями в основном штампе
    /// </summary>
    public partial class StampMainMicrostation
    {
        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        private IEnumerable<IStampChangeSignatureMicrostation> GetStampChangeRowsWithoutSignatures(string personId, string personName) =>
            StampFieldChanges.GetStampRowChangesSignatures().
                              Select(changeRow => changeRow.StampChangeSignatureFields.Select(field => field.Name)).
                              Select(changeNames => GetChangeSignatureField(changeNames, personId, personName)).
                              Where(changeSignature => !changeSignature.NumberChangeElement.Text.IsNullOrWhiteSpace());

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IStampChangeSignatureMicrostation GetChangeSignatureField(IEnumerable<string> changeNames,
                                                                          string personId, string personName)
        {
            var foundElements = FindElementsInStampControls(changeNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>();

            var numberChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsNumberChange(),
                                                    StampFieldType.ChangeSignature);

            var numberOfPlots = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsNumberOfPlots(),
                                                     StampFieldType.ChangeSignature);

            var typeOfChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsTypeOfChange(),
                                                    StampFieldType.ChangeSignature);

            var documentChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsDocumentChange(),
                                                    StampFieldType.ChangeSignature);

            var dateChange = GetFieldFromElements(foundElements, StampFieldChanges.GetFieldsDateChange(),
                                                      StampFieldType.ChangeSignature);

            Func<string, IStampFieldMicrostation> insertSignatureFunc = InsertChangeSignatureFromLibrary(documentChange.ElementStamp,
                                                                                                        dateChange.ElementStamp);

            return new StampChangeSignatureMicrostation(numberChange, numberOfPlots, typeOfChange, documentChange,
                                                        dateChange, personId, personName, insertSignatureFunc);
        }

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private Func<string, IStampFieldMicrostation> InsertChangeSignatureFromLibrary(IElementMicrostation documentChangeElement,
                                                                          IElementMicrostation dateChangeElement) =>
              (string personId) => InsertSignature(personId,
                                                   documentChangeElement.AsTextElementMicrostation,
                                                   dateChangeElement.AsTextElementMicrostation)?.
                                   Map(signature => new StampFieldMicrostation(signature, StampFieldType.PersonSignature));
    }
}
