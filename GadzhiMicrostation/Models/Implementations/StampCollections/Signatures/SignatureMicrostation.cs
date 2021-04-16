using System;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class SignatureMicrostation : StampSignature
    {
        protected SignatureMicrostation(ISignatureLibraryApp signatureLibrary,
                                        IResultAppValue<IStampFieldMicrostation> signature,
                                        Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc)
            :base(signatureLibrary)
        {
            _signature = signature ?? throw new ArgumentNullException(nameof(signature));
            InsertSignatureFunc = insertSignatureFunc ?? throw new ArgumentNullException(nameof(insertSignatureFunc));
        }

        /// <summary>
        /// Подпись. Элемент Word
        /// </summary>
        private readonly IResultAppValue<IStampFieldMicrostation> _signature;

        /// <summary>
        /// Подпись
        /// </summary>
        public override IResultAppValue<IStampField> Signature => _signature;

        /// <summary>
        /// Функция вставки подписи
        /// </summary>
        protected Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> InsertSignatureFunc { get; }

        /// <summary>
        /// Подпись после удаления
        /// </summary>
        protected abstract IStampSignature SignatureDeleted { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid => Signature.OkStatus;

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public override IStampSignature DeleteSignature() =>
            _signature.
            ResultVoidOk(signature => signature.ElementStamp.Remove()).
            Map(_ => SignatureDeleted);

        /// <summary>
        /// Получить пустое поле с подписью
        /// </summary>
        public static IResultAppValue<IStampFieldMicrostation> GetNotInitializedSignature(string personName) =>
            new ResultAppValue<IStampFieldMicrostation>(null, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                       $"Подпись не инициализирована {personName ?? String.Empty}"));
    }
}
