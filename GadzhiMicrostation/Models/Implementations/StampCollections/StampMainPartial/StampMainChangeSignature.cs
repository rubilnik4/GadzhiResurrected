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
    /// Строки с изменениями в основном штампе
    /// </summary>
    public partial class StampMainMicrostation
    {
        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        private IEnumerable<IStampChangeSignatureMicrostation> GetStampChangeRowsWithoutSignatures(string personId) =>
            StampFieldChanges.GetStampRowChangesSignatures().
                              Select(changeRow => changeRow.StampChangeSignatureFields.Select(field => field.Name)).
                              Select(changeNames => GetChangeSignatureField(changeNames, personId));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IStampChangeSignatureMicrostation GetChangeSignatureField(IEnumerable<string> changeNames, string personId)
        {
            var foundElements = FindElementsInStampFields(changeNames, ElementMicrostationType.TextElement).
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

            return new StampChangeSignatureMicrostation(numberChange, numberOfPlots, typeOfChange, 
                                                        documentChange, dateChange, personId);
        }

        /// <summary>
        /// Получить строку с ответственным лицом с подписью
        /// </summary>
        private IStampChangeSignatureMicrostation GetStampChangeRowWithSignatures(IStampChangeSignatureMicrostation changeSignature, string personId) =>
            new StampChangeSignatureMicrostation(changeSignature.NumberChange, changeSignature.NumberOfPlots, 
                                                 changeSignature.TypeOfChange, changeSignature.DocumentChange,
                                                 new StampFieldMicrostation(InsertChangeSignatureFromLibrary(changeSignature, personId),
                                                                            StampFieldType.PersonSignature),
                                                 changeSignature.DateChange, changeSignature.AttributePersonId);

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private ICellElementMicrostation InsertChangeSignatureFromLibrary(IStampChangeSignatureMicrostation changeSignature, string personId) =>
           InsertSignature(personId, changeSignature.DocumentChangeElement, changeSignature.DateChangeElement);
    }
}
