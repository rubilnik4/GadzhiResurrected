using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public class ApprovalSignatureMicrostation : SignatureMicrostation, IStampApproval
    {
        public ApprovalSignatureMicrostation(ISignatureLibraryApp signatureLibrary,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                         IStampTextField departmentApproval, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : this(signatureLibrary, GetNotInitializedSignature(responsiblePerson.MaxLengthWord), insertSignatureFunc,
                   departmentApproval, responsiblePerson, dateSignature)
        { }

        public ApprovalSignatureMicrostation(ISignatureLibraryApp signatureLibrary, IResultAppValue<IStampField> signature,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                         IStampTextField departmentApproval, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : base(signatureLibrary, signature, insertSignatureFunc)
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
        /// Вставить подпись
        /// </summary>
        public override IStampSignature InsertSignature(ISignatureFileApp signatureFile) =>
            new ApprovalSignatureMicrostation(SignatureLibrary, InsertSignatureFunc.Invoke(signatureFile), InsertSignatureFunc,
                                          Department, ResponsiblePerson, DateSignature);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature DeleteSignature() =>
            new ApprovalSignatureMicrostation(SignatureLibrary, InsertSignatureFunc, Department, ResponsiblePerson, DateSignature);
    }
}
