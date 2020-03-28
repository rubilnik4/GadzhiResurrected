using GadzhiApplicationCommon.FunctionalExtensions;
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
                                       Select(personRow => personRow.StampPersonSignatureFields.Select(field => field.Name)).
                                       Select(personNames => GetPersonSignatureField(personNames));


        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IStampPersonSignatureMicrostation GetPersonSignatureField(IEnumerable<string> personNames)
        {
            var foundElements = FindElementsInStampFields(personNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>();

            var actionType = GetFieldFromElements(foundElements, StampFieldPersonSignatures.GetFieldsActionType(),
                                                  StampFieldType.PersonSignature);

            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldPersonSignatures.GetFieldsResponsiblePerson(),
                                                         StampFieldType.PersonSignature);

            var dateSignature = GetFieldFromElements(foundElements, StampFieldPersonSignatures.GetFieldsDateSignature(),
                                                     StampFieldType.PersonSignature);

            Func<string, IStampFieldMicrostation> insertSignatureFunc = InsertPersonSignatureFromLibrary(responsiblePerson.ElementStamp,
                                                                                                         dateSignature.ElementStamp);

            return new StampPersonSignatureMicrostation(actionType, responsiblePerson, dateSignature, insertSignatureFunc);
        }

        /// <summary>
        /// Функция вставки подписей из библиотеки
        /// </summary>  
        private Func<string, IStampFieldMicrostation> InsertPersonSignatureFromLibrary(IElementMicrostation responsiblePersonElement,
                                                                                       IElementMicrostation dateSignatureElement) =>
            (string personId) => InsertSignature(personId,
                                                 responsiblePersonElement.AsTextElementMicrostation,
                                                 dateSignatureElement.AsTextElementMicrostation)?.
                                 Map(signature => new StampFieldMicrostation(signature, StampFieldType.PersonSignature));
    }
}
