using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Microstation
    /// </summary>
    public class PersonSignatureMicrostation : SignatureMicrostation, IStampPerson
    {
        public PersonSignatureMicrostation(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier,
                                           Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                           IStampTextField actionType, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : this(signatureLibrary, stampIdentifier, GetNotInitializedSignature(responsiblePerson.MaxLengthWord), insertSignatureFunc,
                   actionType, responsiblePerson, dateSignature)
        { }

        public PersonSignatureMicrostation(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier, IResultAppValue<IStampFieldMicrostation> signature,
                                           Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                           IStampTextField actionType, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : base(signatureLibrary, stampIdentifier, signature, insertSignatureFunc)
        {
            ResponsiblePerson = responsiblePerson ?? throw new ArgumentNullException(nameof(responsiblePerson));
            ActionType = actionType;
            DateSignature = dateSignature;
        }

        /// <summary>
        /// Тип действия
        /// </summary>
        public IStampTextField ActionType { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>
        public IStampTextField ResponsiblePerson { get; }

        /// <summary>
        /// Дата
        /// </summary>
        public IStampTextField DateSignature { get; }

        /// <summary>
        /// Подпись после удаления
        /// </summary>
        protected override IStampSignature SignatureDeleted =>
             new PersonSignatureMicrostation(SignatureLibrary, StampIdentifier, InsertSignatureFunc, ActionType, ResponsiblePerson, DateSignature);

        /// <summary>
        /// Необходимо ли вставлять подпись в поле
        /// </summary>
        public override bool IsAbleToInsert =>
            base.IsAbleToInsert && !ResponsiblePerson.Text.IsNullOrWhiteSpace();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature InsertSignature(ISignatureFileApp signatureFile) =>
            new PersonSignatureMicrostation(SignatureLibrary, StampIdentifier, InsertSignatureFunc.Invoke(signatureFile), InsertSignatureFunc,
                                        ActionType, ResponsiblePerson, DateSignature);
    }
}
