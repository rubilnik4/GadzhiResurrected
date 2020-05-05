using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
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
                                Select(GetApprovalSignatureField);

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IStampApprovalSignatureMicrostation GetApprovalSignatureField(IEnumerable<string> approvalNames)
        {
            var foundElements = FindElementsInStampControls(approvalNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>().
                                ToList();

            var actionType = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDepartmentApproval(), StampFieldType.ApprovalSignature);
            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsResponsiblePerson(), StampFieldType.ApprovalSignature);
            var dateSignature = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDateSignature(), StampFieldType.ApprovalSignature);
            var insertSignatureFunc = InsertApprovalSignature(responsiblePerson.ElementStamp, dateSignature.ElementStamp);

            return new StampApprovalSignatureMicrostation(actionType, responsiblePerson, dateSignature, insertSignatureFunc);
        }

        /// <summary>
        /// Функция вставки подписей из библиотеки
        /// </summary>      
        private Func<string, IResultAppValue<IStampFieldMicrostation>> InsertApprovalSignature(IElementMicrostation responsiblePersonElement,
                                                                                               IElementMicrostation dateSignatureElement) =>
            personId =>
                InsertSignature(personId, responsiblePersonElement.AsTextElementMicrostation, dateSignatureElement.AsTextElementMicrostation,
                                responsiblePersonElement.AsTextElementMicrostation.Text)?.
                ResultValueOk(signature => new StampFieldMicrostation(signature, StampFieldType.ApprovalSignature));
    }
}
