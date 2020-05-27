using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с согласованием Microstation
    /// </summary>
    public class StampApprovalMicrostation : StampSignatureMicrostation, IStampApproval
    {
        public StampApprovalMicrostation(ISignatureLibraryApp signatureLibrary,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                         IStampTextField departmentApproval, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : this(signatureLibrary, GetNotInitializedSignature(responsiblePerson.Text), insertSignatureFunc,
                   departmentApproval, responsiblePerson, dateSignature)
        { }

        public StampApprovalMicrostation(ISignatureLibraryApp signatureLibrary, IResultAppValue<IStampField> signature,
                                         Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                         IStampTextField departmentApproval, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : base(signatureLibrary, signature, insertSignatureFunc)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            DepartmentApproval = departmentApproval;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Отдел согласования
        /// </summary>
        public IStampTextField DepartmentApproval { get; }

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
            new StampApprovalMicrostation(SignatureLibrary, InsertSignatureFunc.Invoke(signatureFile), InsertSignatureFunc,
                                          DepartmentApproval, ResponsiblePerson, DateSignature);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature DeleteSignature() =>
            new StampApprovalMicrostation(SignatureLibrary, InsertSignatureFunc, DepartmentApproval, ResponsiblePerson, DateSignature);
    }
}
