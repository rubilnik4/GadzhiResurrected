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
    /// Строки с согласованиями
    /// </summary>
    public partial class StampMainMicrostation
    {
        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        private IEnumerable<IStampApprovalSignatureMicrostation> GetStampApprovalRowsWithoutSignatures() =>
            StampFieldApprovals.GetStampRowApprovalSignatures().
                                Select(approvalRow => approvalRow.StampApprovalSignatureFields.Select(field => field.Name)).
                                Select(approvalNames => GetApprovalSignatureField(approvalNames));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IStampApprovalSignatureMicrostation GetApprovalSignatureField(IEnumerable<string> approvalNames)
        {
            var foundElements = FindElementsInStampFields(approvalNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>();

            IStampFieldMicrostation actionType = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDepartmentApproval(),
                                                 StampFieldType.ApprovalSignature);

            IStampFieldMicrostation responsiblePerson = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsResponsiblePerson(),
                                                        StampFieldType.ApprovalSignature);

            IStampFieldMicrostation dateSignature = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDateSignature(),
                                                    StampFieldType.ApprovalSignature);

            Func<string, IStampFieldMicrostation> insertSignatureFunc = InsertApprovalSignatureFromLibrary(responsiblePerson.ElementStamp, 
                                                                                                           dateSignature.ElementStamp);
            
            return new StampApprovalSignatureMicrostation(actionType, responsiblePerson, dateSignature, insertSignatureFunc);
        }

        /// <summary>
        /// Функция вставки подписей из библиотеки
        /// </summary>      
        private Func<string, IStampFieldMicrostation> InsertApprovalSignatureFromLibrary(IElementMicrostation responsiblePersonElement,
                                                                                         IElementMicrostation dateSignatureElement) =>
           (string personId) => new StampFieldMicrostation(InsertSignature(personId,
                                                                           responsiblePersonElement.AsTextElementMicrostation,
                                                                           dateSignatureElement.AsTextElementMicrostation),
                                                           StampFieldType.ApprovalSignature);
    }
}
