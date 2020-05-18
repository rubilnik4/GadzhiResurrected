using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureWord : StampSignature<IStampFieldWord>, IStampSignatureWord
    {
        protected StampSignatureWord(IStampFieldWord signature)
        {
            Signature = new ResultAppValue<IStampFieldWord>(signature, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                            "Подпись не инициализирована"));
        }

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
        public override IStampSignature<IStampFieldWord> InsertSignature(SignatureLibrary signatureLibrary) =>
            Signature.
            ResultVoidOk(signature => signature.CellElementStamp.InsertPicture(signatureLibrary)).
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
