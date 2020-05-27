using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureMicrostation : StampSignature
    {
        protected StampSignatureMicrostation(ISignatureLibraryApp signatureLibrary,
                                             IResultAppValue<IStampField> signature,
                                             Func<ISignatureLibraryApp, IResultAppValue<IStampField>> insertSignatureFunc)
            :base(signatureLibrary)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
            InsertSignatureFunc = insertSignatureFunc ?? throw new ArgumentNullException(nameof(insertSignatureFunc));
        }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IResultAppValue<IStampField> Signature { get; }

        /// <summary>
        /// Функция вставки подписи
        /// </summary>
        protected Func<ISignatureLibraryApp, IResultAppValue<IStampField>> InsertSignatureFunc { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid() => Signature.OkStatus;

        /// <summary>
        /// Получить пустое поле с подписью
        /// </summary>
        public static IResultAppValue<IStampField> GetNotInitializedSignature(string personName) =>
            new ResultAppValue<IStampField>(null, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                       $"Подпись не инициализирована {personName ?? String.Empty}"));
    }
}
