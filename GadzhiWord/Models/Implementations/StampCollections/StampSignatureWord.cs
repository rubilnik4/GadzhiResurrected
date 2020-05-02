using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Enums;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extentions.Functional;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureWord : StampSignature<IStampFieldWord>, IStampSignatureWord
    {
        protected StampSignatureWord(IStampFieldWord signature, string signaturePath)
        {
            SignaturePath = signaturePath ?? throw new ArgumentNullException(nameof(signaturePath));
            Signature = new ResultAppValue<IStampFieldWord>(signature, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                            "Подпись не инициализирована"));
        }

        /// <summary>
        /// Путь файла подписи
        /// </summary>
        public string SignaturePath { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IResultAppValue<IStampFieldWord> Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature.OkStatus && Signature.Value.CellElementStamp.HasPicture;

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public override IStampSignature<IStampFieldWord> InsertSignature() =>
            Signature.
            ResultVoidOk(signature => signature.CellElementStamp.InsertPicture(SignaturePath)).
            Map(_ => this);

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        public override void DeleteSignature() =>
            Signature.
            ResultVoidOk(signature => signature.CellElementStamp.DeleteAllPictures());
    }
}
