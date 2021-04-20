using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.SignatureCreating;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями Word
    /// </summary>
    public partial class StampWord
    {
        /// <summary>
        /// Фабрика создания подписей Word
        /// </summary>
        protected override ISignatureCreating SignatureCreating  => 
            new SignatureCreatingWord(TableStamp, StampSettings.Id, TableApprovalPerformers, TableApprovalChief, this, 
                                      StampDocumentType, SignaturesSearching, StampSettings.PersonId);

        /// <summary>
        /// Вставить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> InsertSignatures() =>
            StampSignatureFields.GetSignatures().
            ResultValueOkBind(GetStampSignaturesByIds).
            ToResultCollection();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public override IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures) =>
            signatures.
            Select(signature => signature.DeleteSignature()).
            ToList().
            Map(signaturesDeleted => new ResultAppCollection<IStampSignature>
                                     (signaturesDeleted,
                                      signaturesDeleted.SelectMany(signature => signature.Signature.Errors),
                                      new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи для удаления не инициализированы")));

        /// <summary>
        /// Получить элементы подписей из базы по их идентификационным номерам
        /// </summary>
        private IResultAppCollection<IStampSignature> GetStampSignaturesByIds(IList<IStampSignature> stampSignatures) =>
            new ResultAppCollection<IStampSignature>(stampSignatures).
            ResultValueOk(_ => stampSignatures.Select(signatureStamp => new SignatureFileRequest(signatureStamp.SignatureLibrary.PersonId,
                                                                                                 signatureStamp.IsVertical))).
            ResultValueOkBind(personIds => SignaturesSearching.GetSignaturesByIds(personIds)).
            ResultValueContinue(signaturesFile => signaturesFile.Count == stampSignatures.Count,
                okFunc: signaturesFile => signaturesFile,
                badFunc: signaturesFile => new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                "Количество подписей в файле не совпадает с загруженным из базы данных")).
            ResultValueOk(signaturesFile =>
                stampSignatures.Zip(signaturesFile,
                                    (signatureStamp, signatureFile) => signatureStamp.InsertSignature(signatureFile))).
            ToResultCollection();
    }
}