using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
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
    public class StampApprovalSignatureMicrostation : StampSignatureMicrostation,
                                                      IStampApprovalSignatureMicrostation
    {
        public StampApprovalSignatureMicrostation(IStampFieldMicrostation departmentApproval, IStampFieldMicrostation responsiblePerson,
                                                  IStampFieldMicrostation dateSignature,
                                                  Func<string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : base(insertSignatureFunc)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            DepartmentApproval = departmentApproval;
            DateSignature = dateSignature;
        }


        /// <summary>
        /// Отдел согласования
        /// </summary>
        public IStampFieldMicrostation DepartmentApproval { get; }

        /// <summary>
        /// Отдел согласования. Элемент
        /// </summary>
        public ITextElementMicrostation DepartmentApprovalElement => DepartmentApproval.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampFieldMicrostation ResponsiblePerson { get; }

        /// <summary>
        /// Ответственное лицо. Элемент
        /// </summary>
        public ITextElementMicrostation ResponsiblePersonElement => ResponsiblePerson.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Дата
        /// </summary>
        public IStampFieldMicrostation DateSignature { get; }

        /// <summary>
        /// Дата. Элемент
        /// </summary>
        public ITextElementMicrostation DateSignatureElement => DateSignature.ElementStamp.AsTextElementMicrostation;

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public override string PersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName => ResponsiblePersonElement.Text;
    }
}
