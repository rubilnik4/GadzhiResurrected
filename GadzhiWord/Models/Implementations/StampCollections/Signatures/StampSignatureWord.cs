using System;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Models.Interfaces.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureWord : StampSignature<IStampFieldWord>, IStampSignatureWord
    {
        protected StampSignatureWord(IStampFieldWord signature,  ISignatureLibraryApp signatureLibrary)
        {
            Signature = new ResultAppValue<IStampFieldWord>(signature, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                            "Подпись не инициализирована"));
            PersonId = signatureLibrary?.PersonId ?? throw new ArgumentNullException(nameof(signatureLibrary));
            PersonInformation = signatureLibrary.PersonInformation;
        }

        /// <summary>
        /// Идентификатор личности
        /// </summary>    
        public override string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public override PersonInformationApp PersonInformation { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IResultAppValue<IStampFieldWord> Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid() => Signature.OkStatus && Signature.Value.CellElementStamp.HasPicture;

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldWord> InsertSignature(ISignatureFileApp signatureFile) =>
            Signature.
            ResultVoidOk(signature => signature.CellElementStamp.InsertPicture(signatureFile.SignatureFilePath)).
            Map(_ => this);

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        public override IStampSignature<IStampFieldWord> DeleteSignature() =>
            Signature.
            ResultVoidOk(signature => signature.CellElementStamp.DeleteAllPictures()).
            Map(_ => this);
    }
}
