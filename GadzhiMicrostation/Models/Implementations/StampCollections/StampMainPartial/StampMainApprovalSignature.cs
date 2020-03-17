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

            var actionType = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDepartmentApproval(),
                                                  StampFieldType.ApprovalSignature);

            var responsiblePerson = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsResponsiblePerson(),
                                                         StampFieldType.ApprovalSignature);

            var dateSignature = GetFieldFromElements(foundElements, StampFieldApprovals.GetFieldsDateSignature(),
                                                     StampFieldType.ApprovalSignature);

            return new StampApprovalSignatureMicrostation(actionType, responsiblePerson, dateSignature);
        }

        /// <summary>
        /// Получить строку с согласованием
        /// </summary>
        private IStampApprovalSignatureMicrostation GetStampApprovalRowWithSignatures(IStampApprovalSignatureMicrostation approvalSignature) =>
            new StampApprovalSignatureMicrostation(approvalSignature.DepartmentApproval, approvalSignature.ResponsiblePerson,
                                                 new StampFieldMicrostation(InsertapprovalSignatureFromLibrary(approvalSignature),
                                                                            StampFieldType.ApprovalSignature),
                                                 approvalSignature.DateSignature);

        /// <summary>
        /// Вставить подписи из библиотеки
        /// </summary>      
        private ICellElementMicrostation InsertapprovalSignatureFromLibrary(IStampApprovalSignatureMicrostation approvalSignature) =>
           InsertSignature(approvalSignature.AttributePersonId,                           
                           approvalSignature.ResponsiblePersonElement,
                           approvalSignature.DateSignatureElement);
    }
}
