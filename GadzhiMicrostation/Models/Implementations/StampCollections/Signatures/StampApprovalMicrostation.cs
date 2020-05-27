using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public class StampApprovalMicrostation : StampSignatureMicrostation, IStampApprovalMicrostation
    {
        public StampApprovalMicrostation(IStampFieldMicrostation departmentApproval, IStampFieldMicrostation responsiblePerson,
                                         IStampFieldMicrostation dateSignature,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            : this(departmentApproval, responsiblePerson, dateSignature, insertSignatureFunc,
                   GetNotInitializedSignature(responsiblePerson.ElementStamp.AsTextElementMicrostation.Text))
        { }

        public StampApprovalMicrostation(IStampFieldMicrostation departmentApproval, IStampFieldMicrostation responsiblePerson,
                                         IStampFieldMicrostation dateSignature,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
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
        public override PersonInformationApp PersonInformation => new PersonInformationApp(ResponsiblePersonElement.Text,
                                                                                     String.Empty, String.Empty,
                                                                                     DepartmentApprovalElement.Text);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> InsertSignature(ISignatureFileApp signatureFile) =>
            new StampApprovalMicrostation(DepartmentApproval, ResponsiblePerson, DateSignature, InsertSignatureFunc,
                                          InsertSignatureFunc.Invoke(signatureFile));

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature<IStampFieldMicrostation> DeleteSignature() =>
            new StampApprovalMicrostation(DepartmentApproval, ResponsiblePerson, DateSignature, InsertSignatureFunc);
    }
}
