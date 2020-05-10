using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public class StampApprovalMicrostation : StampSignatureMicrostation, IStampApprovalMicrostation
    {
        public StampApprovalMicrostation(IStampFieldMicrostation departmentApproval,
                                                  IStampFieldMicrostation responsiblePerson,
                                                  IStampFieldMicrostation dateSignature,
                                                  Func<string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : this(departmentApproval, responsiblePerson, dateSignature, insertSignatureFunc,
                   GetNotInitializedSignature(responsiblePerson.ElementStamp.AsTextElementMicrostation.Text))
        { }

        public StampApprovalMicrostation(IStampFieldMicrostation departmentApproval, IStampFieldMicrostation responsiblePerson,
                                                  IStampFieldMicrostation dateSignature,
                                                  Func<string, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                                  IResultAppValue<IStampFieldMicrostation> signature)
            : base(insertSignatureFunc, signature)
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
        /// идентификатор личности
        /// </summary>    
        public override string PersonId => ResponsiblePerson.ElementStamp.AttributePersonId;

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override string PersonName => ResponsiblePersonElement.Text;

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> InsertSignature() =>
            new StampApprovalMicrostation(DepartmentApproval, ResponsiblePerson, DateSignature, InsertSignatureFunc,
                                                   InsertSignatureFunc.Invoke(PersonId));

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> DeleteSignature() =>
            new StampApprovalMicrostation(DepartmentApproval, ResponsiblePerson, DateSignature, InsertSignatureFunc);
    }
}
