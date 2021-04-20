using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public class ApprovalSignatureMicrostation : SignatureMicrostation, IStampApproval
    {
        public ApprovalSignatureMicrostation(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                         IStampTextField departmentApproval, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : this(signatureLibrary, stampIdentifier, GetNotInitializedSignature(responsiblePerson.MaxLengthWord), insertSignatureFunc,
                   departmentApproval, responsiblePerson, dateSignature)
        { }

        public ApprovalSignatureMicrostation(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier, IResultAppValue<IStampFieldMicrostation> signature,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                         IStampTextField departmentApproval, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : base(signatureLibrary, stampIdentifier, signature, insertSignatureFunc)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            Department = departmentApproval;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public IStampTextField Department { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampTextField ResponsiblePerson { get; }


        /// <summary>
        /// Дата
        /// </summary>
        public IStampTextField DateSignature { get; }

        /// <summary>
        /// Удалить подпись
        /// </summary>
        protected override IStampSignature SignatureDeleted =>
            new ApprovalSignatureMicrostation(SignatureLibrary, StampIdentifier, InsertSignatureFunc, Department, ResponsiblePerson, DateSignature);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature InsertSignature(ISignatureFileApp signatureFile) =>
            new ApprovalSignatureMicrostation(SignatureLibrary, StampIdentifier, InsertSignatureFunc.Invoke(signatureFile), InsertSignatureFunc,
                                          Department, ResponsiblePerson, DateSignature);
    }
}
