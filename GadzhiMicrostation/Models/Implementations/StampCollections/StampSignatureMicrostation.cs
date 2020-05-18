using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая структура подписи Microstation
    /// </summary>
    public abstract class StampSignatureMicrostation : StampSignature<IStampFieldMicrostation>
    {
        protected StampSignatureMicrostation(Func<SignatureLibrary, IResultAppValue<IStampFieldMicrostation>> insertSignatureFunc,
                                             IResultAppValue<IStampFieldMicrostation> signature)
        {
            InsertSignatureFunc = insertSignatureFunc ?? throw new ArgumentNullException(nameof(insertSignatureFunc));
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
        }

        /// <summary>
        /// Функция вставки подписи
        /// </summary>
        protected Func<SignatureLibrary, IResultAppValue<IStampFieldMicrostation>> InsertSignatureFunc { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public override IResultAppValue<IStampFieldMicrostation> Signature { get; }

        /// <summary>
        /// Подпись. Элемент
        /// </summary>
        public IResultAppValue<ICellElementMicrostation> SignatureElement =>
            Signature.ResultValueOk(signature => signature.ElementStamp.AsCellElementMicrostation);

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public override bool IsSignatureValid() => Signature.OkStatus;

        /// <summary>
        /// Получить пустое поле с подписью
        /// </summary>
        public static IResultAppValue<IStampFieldMicrostation> GetNotInitializedSignature(string personName) =>
            new ResultAppValue<IStampFieldMicrostation>(null, new ErrorApplication(ErrorApplicationType.SignatureNotFound,
                                                                                   $"Подпись не инициализирована {personName}"));
    }
}
