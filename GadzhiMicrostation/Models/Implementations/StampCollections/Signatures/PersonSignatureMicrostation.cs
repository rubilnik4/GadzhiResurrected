using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Строка с ответственным лицом и подписью Microstation
    /// </summary>
    public class PersonSignatureMicrostation : SignatureMicrostation, IStampPerson
    {
        public PersonSignatureMicrostation(ISignatureLibraryApp signatureLibrary,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                       IStampTextField actionType, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : this(signatureLibrary, GetNotInitializedSignature(responsiblePerson.MaxLengthWord), insertSignatureFunc,
                   actionType, responsiblePerson, dateSignature)
        { }

        public PersonSignatureMicrostation(ISignatureLibraryApp signatureLibrary, IResultAppValue<IStampField> signature,
                                       Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc,
                                       IStampTextField actionType, IStampTextField responsiblePerson, IStampTextField dateSignature)
            : base(signatureLibrary, signature, insertSignatureFunc)
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
        /// Необходимо ли вставлять подпись в поле
        /// </summary>
        public override bool IsAbleToInsert =>
            base.IsAbleToInsert && !ResponsiblePerson.Text.IsNullOrWhiteSpace();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature InsertSignature(ISignatureFileApp signatureFile) =>
            new PersonSignatureMicrostation(SignatureLibrary, InsertSignatureFunc.Invoke(signatureFile), InsertSignatureFunc,
                                        ActionType, ResponsiblePerson, DateSignature);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature DeleteSignature() =>
            new PersonSignatureMicrostation(SignatureLibrary, InsertSignatureFunc, ActionType, ResponsiblePerson, DateSignature);
    }
}
