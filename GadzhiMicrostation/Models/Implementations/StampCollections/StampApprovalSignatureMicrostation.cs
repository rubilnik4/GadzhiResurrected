using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public class StampApprovalSignatureMicrostation : StampApprovalSignature<IStampFieldMicrostation>,
                                                      IStampApprovalSignatureMicrostation
    {
        public StampApprovalSignatureMicrostation(IStampFieldMicrostation approvalSignature, IStampFieldMicrostation responsiblePerson,
                                                  IStampFieldMicrostation dateSignature)
            : this(approvalSignature, responsiblePerson, null, dateSignature) { }

        public StampApprovalSignatureMicrostation(IStampApprovalSignatures<IStampFieldMicrostation> approvalSignature)
            : this(approvalSignature?.DepartmentApproval, approvalSignature?.ResponsiblePerson,
                  approvalSignature?.Signature, approvalSignature?.DateSignature)
        {
            if (approvalSignature == null) throw new ArgumentNullException(nameof(approvalSignature));
        }

        public StampApprovalSignatureMicrostation(IStampFieldMicrostation departmentApproval, IStampFieldMicrostation responsiblePerson,
                                                  IStampFieldMicrostation signature, IStampFieldMicrostation dateSignature)
        {
            DepartmentApproval = departmentApproval;
            ResponsiblePerson = responsiblePerson;
            Signature = signature;
            DateSignature = dateSignature;
        }


        /// <summary>
        /// Отдел согласования
        /// </summary>
        public override IStampFieldMicrostation DepartmentApproval { get; }

        /// <summary>
        /// Отдел согласования. Элемент
        /// </summary>
        public ITextElementMicrostation DepartmentApprovalElement => DepartmentApproval.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public override IStampFieldMicrostation ResponsiblePerson { get; }

        /// <summary>
        /// Ответственное лицо. Элемент
        /// </summary>
        public ITextElementMicrostation ResponsiblePersonElement => ResponsiblePerson.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Подпись
        /// </summary>
        public override IStampFieldMicrostation Signature { get; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        public ICellElementMicrostation SignatureElement => Signature.ElementStamp.AsCellElementMicrostation;

        /// <summary>
        /// Дата
        /// </summary>
        public override IStampFieldMicrostation DateSignature { get; }

        /// <summary>
        /// Дата. Элемент
        /// </summary>
        public ITextElementMicrostation DateSignatureElement => DateSignature.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string AttributePersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

    }
}
