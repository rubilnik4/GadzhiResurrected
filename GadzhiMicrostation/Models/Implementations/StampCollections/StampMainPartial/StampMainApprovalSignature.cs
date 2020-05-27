using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiMicrostation.Models.Implementations.StampCollections.Signatures;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

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
        private IResultAppCollection<IStampApprovalMicrostation> GetStampApprovalRows() =>
            GetStampSignatureRows(StampFieldType.ApprovalSignature, GetApprovalSignatureField).
            Map(signatureRows => new ResultAppCollection<IStampApprovalMicrostation>(signatureRows, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                                                       "Штамп подписей согласования не найден")));

        /// <summary>
        /// Преобразовать элементы Microstation в строку подписей
        /// </summary>
        private IResultAppValue<IStampApprovalMicrostation> GetApprovalSignatureField(IEnumerable<string> approvalNames)
        {
            var foundElements = FindElementsInStampControls(approvalNames, ElementMicrostationType.TextElement).
                                Cast<ITextElementMicrostation>().
                                ToList();
            if (foundElements.Count == 0) return new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Поля подписей согласования не найдены").
                                                 ToResultApplicationValue<IStampApprovalMicrostation>();

            var actionType = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDepartmentApproval(), StampFieldType.ApprovalSignature);
            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsResponsiblePerson(), StampFieldType.ApprovalSignature);
            var dateSignature = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDateSignature(), StampFieldType.ApprovalSignature);

            var insertSignatureFunc = InsertSignatureFunc(responsiblePerson.ElementStamp, dateSignature.ElementStamp, StampFieldType.ApprovalSignature);

            return new StampApprovalMicrostation(actionType, responsiblePerson, dateSignature, insertSignatureFunc).
                   Map(stampApprovalSignature => new ResultAppValue<IStampApprovalMicrostation>(stampApprovalSignature));
        }
    }
}
