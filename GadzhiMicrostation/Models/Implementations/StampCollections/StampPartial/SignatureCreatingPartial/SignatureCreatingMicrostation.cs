using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.StampCollections.Fields;
using GadzhiMicrostation.Models.Implementations.StampFieldNames;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampPartial;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.StampPartial.SignatureCreatingPartial
{
    using InsertSignatureFunc = Func<string, ITextElementMicrostation, ITextElementMicrostation,
                                            IResultAppValue<ICellElementMicrostation>>;
    /// <summary>
    /// Фабрика создания подписей Microstation
    /// </summary>
    public partial class SignatureCreatingMicrostation : SignatureCreating
    {
        public SignatureCreatingMicrostation(IStampFieldsMicrostation stampFields,
                                             InsertSignatureFunc insertSignatureByFields,
                                             SignaturesSearching signaturesSearching, string personId)
            : base(signaturesSearching, personId)
        {
            _stampFields = stampFields ?? throw new ArgumentNullException(nameof(stampFields));
            _insertSignatureByFields = insertSignatureByFields ?? throw new ArgumentNullException(nameof(insertSignatureByFields));
        }

        /// <summary>
        /// Поля штампа Microstation
        /// </summary>
        private readonly IStampFieldsMicrostation _stampFields;

        /// <summary>
        /// Получить поля штампа на основе элементов Microstation
        /// </summary>
        private readonly InsertSignatureFunc _insertSignatureByFields;

        /// <summary>
        /// Получить строки с ответственным лицом/отделом без подписи
        /// </summary>
        private static IEnumerable<TSignatureField> GetStampSignatureRows<TSignatureField>(StampFieldType stampFieldType,
                                                                                          Func<IEnumerable<string>, IResultAppValue<TSignatureField>> getSignatureField)
                where TSignatureField : IStampSignature =>
            StampFieldSignatures.GetFieldsBySignatureType(stampFieldType).
            Select(getSignatureField).
            Where(resultSignature => resultSignature.OkStatus).
            Select(resultSignature => resultSignature.Value);

        /// <summary>
        /// Функция вставки подписей из библиотеки
        /// </summary>      
        protected Func<ISignatureLibraryApp, IResultAppValue<IStampFieldMicrostation>> InsertSignatureFunc
            (IElementMicrostation previousElement, IElementMicrostation nextElement, StampFieldType stampFieldType) =>
            signatureLibrary =>
                _insertSignatureByFields(signatureLibrary.PersonId, previousElement.AsTextElementMicrostation, nextElement.AsTextElementMicrostation).
                ResultValueOk(signature => new StampFieldMicrostation(signature, stampFieldType));
    }
}